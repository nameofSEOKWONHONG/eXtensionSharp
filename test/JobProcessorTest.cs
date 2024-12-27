// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net.Http;
// using System.Text;
// using System.Threading.Tasks;
// using eXtensionSharp.Job;
// using NUnit.Framework;
//
// namespace eXtensionSharp.test;
//
// public class JobProcessorTest
// {
//     [Test, Order(1)]
//     public async Task job_processor_test()
//     {
//         using var processor = new JobProcessor<string>();
//         processor.SetProcess(JobHandler<string>.Instance, item =>
//         {
//             if(item.xIsNotEmpty()) TestContext.Out.WriteLine(item);
//         });
//             
//         var texts = "hello world";
//
//         Parallel.ForEach(texts.ToArray(), item =>
//         {
//             JobHandler<string>.Instance.Enqueue(item.ToString());
//         });
//         await Task.Delay(5000);
//     }
//
//     [Test, Order(2)]
//     public async Task job_processor_test2()
//     {
//         var r1 = new StringBuilder();
//         using var processor = new JobProcessorAsync<string>();
//         processor.SetProcess(JobHandler<string>.Instance, item =>
//         {
//             r1.Append(item);
//             return Task.CompletedTask;
//         });
//             
//         var texts = "hello world";
//
//         Parallel.ForEach(texts.ToArray(), item =>
//         {
//             JobHandler<string>.Instance.Enqueue(item.ToString());
//         });
//
//         await Task.Delay(5000);
//         
//         Assert.Multiple(() =>
//         {
//             Assert.That(r1.Length, Is.EqualTo(texts.Length));
//         });
//     }
// }