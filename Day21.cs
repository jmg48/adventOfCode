namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

[TestFixture]
public class Day21
{
    [Test]
    public void Part1()
    {
        var monkeys = new Dictionary<string, Lazy<long>>();
        foreach (var match in File.ReadLines("C:\\git\\input21.txt").Select(line => Regex.Match(line, @"(\w+): ((\w+) ([+\-*/]) (\w+)|\d+)")))
        {
            monkeys[match.Groups[1].Value] = new Lazy<long>(() =>
            {
                if (long.TryParse(match.Groups[2].Value, out var value))
                {
                    return value;
                }

                checked
                {
                    var arg1 = monkeys[match.Groups[3].Value].Value;
                    var arg2 = monkeys[match.Groups[5].Value].Value;
                    return match.Groups[4].Value switch { "+" => arg1 + arg2, "-" => arg1 - arg2, "*" => arg1 * arg2, "/" => arg1 / arg2 };
                }
            });
        }

        Console.WriteLine(monkeys["root"].Value);
    }

    [Test]
    public void Part2()
    {
        var monkeys = new Dictionary<string, Lazy<MonkeyDo>>();
        var rootArg1 = string.Empty;
        var rootArg2 = string.Empty;
        foreach (var match in File.ReadLines("C:\\git\\input21.txt").Select(line => Regex.Match(line, @"(\w+): ((\w+) ([+\-*/]) (\w+)|\d+)")))
        {
            var name = match.Groups[1].Value;
            var expr = match.Groups[2].Value;
            var arg1 = match.Groups[3].Value;
            var op = match.Groups[4].Value;
            var arg2 = match.Groups[5].Value;

            if (name == "root")
            {
                rootArg1 = arg1;
                rootArg2 = arg2;
            }

            monkeys[name] = new Lazy<MonkeyDo>(() =>
            {
                if (name == "humn")
                {
                    return new MonkeyDo.Unknown("humn");
                }

                if (long.TryParse(expr, out var value))
                {
                    return new MonkeyDo.Constant(value);
                }

                return new MonkeyDo.Arithmetic(monkeys[arg1].Value, op, monkeys[arg2].Value);
            });
        }

        MonkeyDo Reduce(MonkeyDo arg) => arg switch
        {
            MonkeyDo.Unknown unknown => unknown,
            MonkeyDo.Constant constant => constant,
            MonkeyDo.Arithmetic arithmetic => TryEval(new MonkeyDo.Arithmetic(Reduce(arithmetic.Arg1), arithmetic.Op, Reduce(arithmetic.Arg2))),
        };

        MonkeyDo TryEval(MonkeyDo.Arithmetic arithmetic) =>
            arithmetic.Arg1 is MonkeyDo.Constant val1 && arithmetic.Arg2 is MonkeyDo.Constant val2
                ? new MonkeyDo.Constant(arithmetic.Op switch
                {
                    "+" => val1.Value + val2.Value,
                    "-" => val1.Value - val2.Value,
                    "*" => val1.Value * val2.Value,
                    "/" => val1.Value / val2.Value,
                })
                : arithmetic;

        var lhs = Reduce(monkeys[rootArg1].Value);
        var rhs = Reduce(monkeys[rootArg2].Value);

        while (lhs is MonkeyDo.Arithmetic expression && rhs is MonkeyDo.Constant constant)
        {
            checked
            {
                Console.WriteLine(lhs);
                Console.WriteLine(rhs);
                Console.WriteLine();

                if (expression.Arg1 is MonkeyDo.Constant arg1)
                {
                    lhs = expression.Arg2;
                    rhs = expression.Op switch
                    {
                        "+" => new MonkeyDo.Constant(constant.Value - arg1.Value),
                        "-" => new MonkeyDo.Constant(arg1.Value - constant.Value),
                        "*" => new MonkeyDo.Constant(constant.Value / arg1.Value),
                        "/" => new MonkeyDo.Constant(arg1.Value / constant.Value),
                    };
                }
                else if (expression.Arg2 is MonkeyDo.Constant arg2)
                {
                    lhs = expression.Arg1;
                    rhs = expression.Op switch
                    {
                        "+" => new MonkeyDo.Constant(constant.Value - arg2.Value),
                        "-" => new MonkeyDo.Constant(constant.Value + arg2.Value),
                        "*" => new MonkeyDo.Constant(constant.Value / arg2.Value),
                        "/" => new MonkeyDo.Constant(constant.Value * arg2.Value),
                    };
                }
            }
        }

        Console.WriteLine($"{rhs} = {lhs}");
    }

    private record MonkeyDo
    {
        public record Unknown(string Symbol) : MonkeyDo;

        public record Constant(long Value) : MonkeyDo;

        public record Arithmetic(MonkeyDo Arg1, string Op, MonkeyDo Arg2) : MonkeyDo;
    }
}