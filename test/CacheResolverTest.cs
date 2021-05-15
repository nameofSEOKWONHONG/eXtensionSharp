using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class CacheResolverTest {
        [SetUp]
        public void SetUp() {
        }
        
        [Test]
        public void cache_resolver_test() {
            var item1 = CacheResolverFactory.Instance.Execute<ResolverTestResolver, ResolverTestDto>();
            
            Thread.Sleep(1000 * 6);
            
            var item2 = CacheResolverFactory.Instance.Execute<ResolverTestResolver, ResolverTestDto>();
            Assert.AreEqual(item1.Id, item2.Id);
        }

        [Test]
        public void cache_resolver_reset_test() {
            var item1 = CacheResolverFactory.Instance.Execute<ResolverTestResolver, ResolverTestDto>();
            
            Thread.Sleep(1000 * 6);
            
            var item2 = CacheResolverFactory.Instance.Execute<ResolverTestResolver, ResolverTestDto>();

            Assert.AreEqual(0, CacheResolverFactory.Instance.Count());
        }
        
        public class ResolverTestResolver : CacheResolverBase<ResolverTestDto> {
            private ResolverTestRepository _repository;

            public ResolverTestResolver() {
                _repository = new ResolverTestRepository();

            }

            public override string InitKey() {
                //TODO : create key data
                var selectedItem = _repository.GetItems().xFirst(m => m.EditDt == _repository.GetItems().Max(mm => mm.EditDt));
                return selectedItem.EditDt.GetHashCode().xValue();
            }

            public override ResolverTestDto GetOrSet() {
                //TODO : implement get or set
                var selectedItem = _repository.GetItems().xFirst(m => m.EditDt == _repository.GetItems().Max(mm => mm.EditDt));
                return selectedItem;
            }

            public override int GetResetInterval() {
                return 5; //second
            }

            public override void Delete() {
                throw new NotImplementedException();
            }
        }
    }

    public class ResolverTestRepository {
        private List<ResolverTestDto> _items = new List<ResolverTestDto>();
        public ResolverTestRepository() {
            _items.Add(new ResolverTestDto() {
                Id = 1,
                Name = "test",
                EditDt = DateTime.Parse("2021-01-01")
            });
            _items.Add(new ResolverTestDto() {
                Id = 2,
                Name = "test2",
                EditDt = DateTime.Parse("2021-01-02")
            });
        }

        public IEnumerable<ResolverTestDto> GetItems() {
            return _items;
        }
    }

    public class ResolverTestDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EditDt { get; set; }
    }
}