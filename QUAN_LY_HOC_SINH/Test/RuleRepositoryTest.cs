using NUnit.Framework;
using Repositories;
using Repositories.Models;
using Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestFixture]
    public class RuleRepositoryTest
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository _genericRepository;
        private IRuleRepository _ruleRepository;
        private IList<Rule> _mockRules;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _unitOfWork = new UnitOfWork();
            _genericRepository = new GenericRepository(_unitOfWork);
            _ruleRepository = new RuleRepository(_unitOfWork);
            _mockRules = new List<Rule>();
        }

        [TearDown]
        public void TearDown()
        {
            using (_unitOfWork.Start())
            {
                foreach (Rule rule in _mockRules)
                {
                    _genericRepository.Delete(rule);
                }
                _unitOfWork.Commit();
            }
            _mockRules.Clear();
        }

        [Test]
        public void FindAllRules__SaveThreeMockRulesAndInvokedFindAllRules__AllThreeMockRulesShouldBeFound()
        {
            // Arrange
            Rule rule1 = CreateOneMockRule(2000, "rule 1", "int", "10");
            Rule rule2 = CreateOneMockRule(2001, "rule 2", "float", "7.5");
            Rule rule3 = CreateOneMockRule(2002, "rule 3", "int", "20");
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(rule1);
                _genericRepository.Save(rule2);
                _genericRepository.Save(rule3);
                _unitOfWork.Commit();
            }
            _mockRules.Add(rule1);
            _mockRules.Add(rule2);
            _mockRules.Add(rule3);

            // Act
            IList<Rule> rules = _ruleRepository.FindAllRules();

            // Assert
            foreach (Rule rule in _mockRules)
            {
                Rule foundRule = rules.Where(x => x.Id == rule.Id).FirstOrDefault();
                Assert.AreNotEqual(null, foundRule);
                Assert.AreEqual(rule.Name, foundRule.Name);
                Assert.AreEqual(rule.Type, foundRule.Type);
                Assert.AreEqual(rule.Value, foundRule.Value);
            }
        }

        [Test]
        public void FindRuleById__SaveOneMockRuleAndFind__TheRuleShouldBeFound()
        {
            // Arrange
            Rule rule = CreateOneMockRule(2000, "rule", "int", "10");
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(rule);
                _unitOfWork.Commit();
            }
            _mockRules.Add(rule);

            // Act
            Rule foundRule = _ruleRepository.FindRuleById(rule.Id);

            // Assert
            Assert.AreNotEqual(null, foundRule);
            Assert.AreEqual(rule.Name, foundRule.Name);
            Assert.AreEqual(rule.Type, foundRule.Type);
            Assert.AreEqual(rule.Value, foundRule.Value);
        }

        private Rule CreateOneMockRule(int id, string name, string type, string value)
        {
            return new Rule
            {
                Id = id,
                Name = name,
                Type = type,
                Value = value
            };
        }
    }
}
