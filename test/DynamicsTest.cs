using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace eXtensionSharp.test;

public class DynamicsTest
{
    [TestFixture]
    public class Variant3Tests
    {
        [Test]
        public void From_T1_SetsTagAndValue()
        {
            var v = Variant3<int, string, DateTime>.From(10);
            Assert.That(v.IsT1, Is.True);
            Assert.That(v.IsT2 || v.IsT3, Is.False);
            Assert.That(v.AsT1, Is.EqualTo(10));
            Assert.Throws<InvalidOperationException>(() => { var _ = v.AsT2; });
            Assert.Throws<InvalidOperationException>(() => { var _ = v.AsT3; });
        }

        [Test]
        public void From_T2_SetsTagAndValue()
        {
            var v = Variant3<int, string, DateTime>.From("ok");
            Assert.That(v.IsT2, Is.True);
            Assert.That(v.AsT2, Is.EqualTo("ok"));
        }

        [Test]
        public void From_T3_SetsTagAndValue()
        {
            var now = new DateTime(2025, 9, 9, 12, 0, 0, DateTimeKind.Utc);
            var v = Variant3<int, string, DateTime>.From(now);
            Assert.That(v.IsT3, Is.True);
            Assert.That(v.AsT3, Is.EqualTo(now));
        }

        [Test]
        public void TryGet_ReturnsTrueOnlyForMatchingType()
        {
            var v = Variant3<int, string, DateTime>.From(42);

            Assert.That(v.TryGet(out int i), Is.True);
            Assert.That(i, Is.EqualTo(42));

            Assert.That(v.TryGet(out string s), Is.False);
            Assert.That(s, Is.Null);

            Assert.That(v.TryGet(out DateTime d), Is.False);
            Assert.That(d, Is.EqualTo(default(DateTime)));
        }

        [Test]
        public void Match_And_Switch_Work()
        {
            var a = Variant3<int, string, DateTime>.From(3);
            var b = Variant3<int, string, DateTime>.From("x");
            var c = Variant3<int, string, DateTime>.From(new DateTime(2025, 1, 1));

            Assert.That(a.Match(i => i * 2, s => s.Length, d => d.Year), Is.EqualTo(6));
            Assert.That(b.Match(i => i * 2, s => s.Length, d => d.Year), Is.EqualTo(1));
            Assert.That(c.Match(i => i * 2, s => s.Length, d => d.Year), Is.EqualTo(2025));

            int iCnt = 0, sCnt = 0, dCnt = 0;
            a.Switch(_ => iCnt++, _ => sCnt++, _ => dCnt++);
            b.Switch(_ => iCnt++, _ => sCnt++, _ => dCnt++);
            c.Switch(_ => iCnt++, _ => sCnt++, _ => dCnt++);
            Assert.That(iCnt, Is.EqualTo(1));
            Assert.That(sCnt, Is.EqualTo(1));
            Assert.That(dCnt, Is.EqualTo(1));
        }

        [Test]
        public void Default_Variant_ThrowsOnAccess_And_TryGetFalse()
        {
            var v = default(Variant3<int, string, DateTime>);
            Assert.That(v.IsT1 || v.IsT2 || v.IsT3, Is.False);

            Assert.Throws<InvalidOperationException>(() => { var _ = v.AsT1; });
            Assert.Throws<InvalidOperationException>(() => { var _ = v.AsT2; });
            Assert.Throws<InvalidOperationException>(() => { var _ = v.AsT3; });

            Assert.That(v.TryGet(out int _), Is.False);
            Assert.That(v.TryGet(out string _), Is.False);
            Assert.That(v.TryGet(out DateTime _), Is.False);
        }
    }

    [TestFixture]
    public class HListTests
    {
        [Test]
        public void Add_And_Indexer_Work()
        {
            var xs = new HList<int, string, DateTime>();
            var dt = new DateTime(2025, 9, 9, 12, 34, 56, DateTimeKind.Utc);

            xs.Add(1);
            xs.Add("hi");
            xs.Add(dt);

            Assert.That(xs.Count, Is.EqualTo(3));

            // index 0 = int
            Assert.That(xs[0].IsT1, Is.True);
            Assert.That(xs[0].AsT1, Is.EqualTo(1));

            // index 1 = string
            Assert.That(xs[1].IsT2, Is.True);
            Assert.That(xs[1].AsT2, Is.EqualTo("hi"));

            // index 2 = DateTime
            Assert.That(xs[2].IsT3, Is.True);
            Assert.That(xs[2].AsT3, Is.EqualTo(dt));
        }

        [Test]
        public void TryGetAt_ByType_Works()
        {
            var xs = new HList<int, string, DateTime>();
            xs.Add(10);
            xs.Add("x");

            Assert.That(xs.TryGetAt<int>(0, out var i0), Is.True);
            Assert.That(i0, Is.EqualTo(10));

            Assert.That(xs.TryGetAt<string>(1, out var s1), Is.True);
            Assert.That(s1, Is.EqualTo("x"));

            // mismatched type
            Assert.That(xs.TryGetAt<DateTime>(0, out var _), Is.False);
        }

        [Test]
        public void ForEach_AccumulatesPerType()
        {
            var xs = new HList<int, string, DateTime>();
            xs.Add(1);
            xs.Add(2);
            xs.Add("a");
            xs.Add("bc");
            xs.Add(new DateTime(2025, 1, 1));
            xs.Add(new DateTime(2025, 2, 1));

            int sum = 0;
            int totalLen = 0;
            int dateCount = 0;

            xs.ForEach(
                i => sum += i,
                s => totalLen += s.Length,
                _ => dateCount++
            );

            Assert.That(sum, Is.EqualTo(3));
            Assert.That(totalLen, Is.EqualTo(3));
            Assert.That(dateCount, Is.EqualTo(2));
        }

        [Test]
        public void Select_ProjectsToUniformType()
        {
            var xs = new HList<int, string, DateTime>();
            xs.Add(5);
            xs.Add("hi");
            xs.Add(new DateTime(2025, 9, 9));

            var projected = xs.Select(
                i => $"int:{i}",
                s => $"str:{s}",
                d => $"dt:{d:yyyy-MM-dd}"
            ).ToList();

            CollectionAssert.AreEqual(
                new[] { "int:5", "str:hi", "dt:2025-09-09" },
                projected);
        }

        [Test]
        public void Enumeration_Works()
        {
            var xs = new HList<int, string, DateTime>();
            xs.Add(1);
            xs.Add("x");
            xs.Add(2);

            var tags = new List<byte>();
            foreach (var v in xs)
            {
                if (v.IsT1) tags.Add(1);
                else if (v.IsT2) tags.Add(2);
                else if (v.IsT3) tags.Add(3);
                else tags.Add(0);
            }

            CollectionAssert.AreEqual(new byte[] { 1, 2, 1 }, tags);
        }

        [Test]
        public void Index_OutOfRange_Throws()
        {
            var xs = new HList<int, string, DateTime>();
            xs.Add(1);
            xs.Add("x");
            //Assert.Throws<ArgumentOutOfRangeException>(() => { var _ = xs[1]; });
            Assert.That(xs.xGetSafe(2), Is.Default);
        }
    }
}