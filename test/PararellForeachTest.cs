using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace eXtensionSharp.test {
    public class PararellForeachTest {
        private List<PararellData> _list = new List<PararellData>();
        
        [SetUp]
        public void Setup() {
            _list.Add(new PararellData() {
                COM_CODE = "30000", FORM_TYPE = "SI030", FORM_SEQ = 0, CONTENTS = "test1"
            });
            _list.Add(new PararellData() {
                COM_CODE = "30000", FORM_TYPE = "SI030", FORM_SEQ = 0, CONTENTS = "test2"
            });
            _list.Add(new PararellData() {
                COM_CODE = "30001", FORM_TYPE = "SI030", FORM_SEQ = 0, CONTENTS = "test1"
            });
            _list.Add(new PararellData() {
                COM_CODE = "30001", FORM_TYPE = "SI030", FORM_SEQ = 0, CONTENTS = "test2"
            });
            _list.Add(new PararellData() {
                COM_CODE = "30001", FORM_TYPE = "SI030", FORM_SEQ = 0, CONTENTS = "test3"
            });
            _list.Add(new PararellData() {
                COM_CODE = "30002", FORM_TYPE = "SI030", FORM_SEQ = 0, CONTENTS = "test1"
            });
            _list.Add(new PararellData() {
                COM_CODE = "30002", FORM_TYPE = "SI030", FORM_SEQ = 0, CONTENTS = "test2"
            });
            _list.Add(new PararellData() {
                COM_CODE = "30002", FORM_TYPE = "SI030", FORM_SEQ = 0, CONTENTS = "test3"
            });
            _list.Add(new PararellData() {
                COM_CODE = "30002", FORM_TYPE = "SI030", FORM_SEQ = 0, CONTENTS = "test4"
            });
            _list.Add(new PararellData() {
                COM_CODE = "30002", FORM_TYPE = "SI030", FORM_SEQ = 0, CONTENTS = "test5"
            });
        }
        
        /// <summary>
        /// 병렬 루프를 위한 자료 생성에 대한 테스트
        /// </summary>
        [Test]
        public void parallel_loop_test() {
            var maps = new Dictionary<string, List<PararellData>>();
            var groups = _list.GroupBy(m => m.COM_CODE);
            groups.xForEach(group => {
                maps.Add(group.Key, _list.Where(m => m.COM_CODE == group.Key).ToList());
            });

            Parallel.ForEach(maps, item => {
                var i = 1;
                item.Value.xForEach(item2 => {
                    item2.FORM_SEQ += i;
                    i++;
                });
            });

            maps.xForEach(item => {
                Console.WriteLine(item.xObjectToJson());
            });            
        }

        /// <summary>
        /// 병렬 루프를 확장 메서드로 진행한 케이스
        /// </summary>
        [Test]
        public void Parallel_loop_extension_test() {
            _list.xPararellForeach(items => {
                return items.GroupBy(m => m.COM_CODE);
            }, (key, datas) => {
                return datas.Where(m => m.COM_CODE == key);
            }, (data, i) => {
                data.FORM_SEQ = i + 1;
                Console.WriteLine(data.xObjectToJson());
            });
        }
    }

    public class PararellData {
        public string COM_CODE { get; set; }
        public string FORM_TYPE { get; set; }
        public int FORM_SEQ { get; set; }
        public string CONTENTS { get; set; }
    }
}