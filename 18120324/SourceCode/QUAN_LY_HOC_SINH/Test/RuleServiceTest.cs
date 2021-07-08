using NUnit.Framework;
using Repositories;
using Repositories.Models;
using Repositories.UnitOfWork;
using Services;
using Services.DTO;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestFixture]
    public class RuleServiceTest
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository _genericRepository;
        private IRuleRepository _ruleRepository;
        private IRuleService _ruleService;
        private IList<Rule> _mockRules;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _unitOfWork = new UnitOfWork();
            _genericRepository = new GenericRepository(_unitOfWork);
            _ruleRepository = new RuleRepository(_unitOfWork);
            _ruleService = new RuleService(_unitOfWork, _genericRepository, _ruleRepository);
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
        public void FindAllRules__AddThreeMockRulesToDatabase__AllThreeMockRulesShouldBeFound()
        {
            // Arrange
            Rule rule1 = CreateRule(2000, "rule 1", "int", "10");
            Rule rule2 = CreateRule(2001, "rule 2", "float", "7.5");
            Rule rule3 = CreateRule(2002, "rule 3", "int", "20");
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
            IList<UpdateRuleDTO> updateRuleDTOs =  _ruleService.FindAllRules();

            // Assert
            foreach (Rule rule in _mockRules)
            {
                UpdateRuleDTO updateRuleDTO = updateRuleDTOs.Where(x => x.Id == rule.Id).FirstOrDefault();
                Assert.AreNotEqual(null, updateRuleDTO);
                AssertRuleAndDTO(rule, updateRuleDTO);
            }
        }

        [Test]
        public void UpdateRule__CreateOneMockRuleAndUpdateIt__TheRuleShouldBeUpdatedSucessfully()
        {
            // Arrange
            Rule rule = CreateRule(2000, "rule", "float", "5.5");
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(rule);
                _unitOfWork.Commit();
            }
            UpdateRuleDTO updateRuleDTO = CreateUpdateRuleDTO(rule.Id, rule.Name, rule.Type, rule.Value, rule.Version);
            updateRuleDTO.Value = "6.0";

            // Act
            _ruleService.UpdateRule(updateRuleDTO);

            // Assert
            using (_unitOfWork.Start())
            {
                rule = _ruleRepository.FindRuleById(updateRuleDTO.Id);
            }
            _mockRules.Add(rule);
            Assert.AreNotEqual(null, rule);
            AssertRuleAndDTO(rule, updateRuleDTO);
        }

        [Test]
        public void UpdateRule__UpdateANonExistRule__ObjectNotExistsExceptionShouldBeThrown()
        {
            // Arrange
            int ruleId;
            using (_unitOfWork.Start())
            {
                do
                {
                    ruleId = new Random().Next(1000, 9999);
                }
                while (_ruleRepository.FindRuleById(ruleId) != null);
            }
            UpdateRuleDTO updateRuleDTO = CreateUpdateRuleDTO(ruleId, "rule", "int", "10");

            // Act
            void updateNonExistRule() => _ruleService.UpdateRule(updateRuleDTO);

            // Assert
            Assert.Throws(typeof(ObjectNotExistsException), updateNonExistRule);
        }

        [Test]
        public void UpdateRule__UpdateOneRuleTypeIntWithAlphabetValue__UnableToCastExceptionnShouldBeThrown()
        {
            // Arrange
            Rule rule = CreateRule(2000, "rule", "int", "5.5");
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(rule);
                _unitOfWork.Commit();
            }
            _mockRules.Add(rule);
            UpdateRuleDTO updateRuleDTO = CreateUpdateRuleDTO(rule.Id, rule.Name, rule.Type, rule.Value, rule.Version);
            updateRuleDTO.Value = "abc";

            // Act
            void updateRuleWithInvalidValue() => _ruleService.UpdateRule(updateRuleDTO);

            // Assert
            Assert.Throws(typeof(UnableToCastException), updateRuleWithInvalidValue);
        }

        [Test]
        public void UpdateRule__UpdateARuleWhichHasBeenUpdated__ObjectHasBeenUpdatedExceptionShouldBeThrown()
        {
            // Arrange
            Rule rule = CreateRule(2000, "rule", "float", "5.5");
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(rule);
                _unitOfWork.Commit();
            }
            UpdateRuleDTO updateRuleDTO = CreateUpdateRuleDTO(rule.Id, rule.Name, rule.Type, rule.Value, rule.Version);
            using (_unitOfWork.Start())
            {
                rule.Value = "6.0";
                _genericRepository.Update(rule);
                _unitOfWork.Commit();
            }
            _mockRules.Add(rule);

            // Act
            void updateARuleWhichHasBeenUpdated() => _ruleService.UpdateRule(updateRuleDTO);

            // Assert
            Assert.Throws(typeof(ObjectHasBeenUpdatedException), updateARuleWhichHasBeenUpdated);
        }

        private Rule CreateRule(int id, string name, string type, string value)
        {
            return new Rule
            {
                Id = id,
                Name = name,
                Type = type,
                Value = value
            };
        }
        private UpdateRuleDTO CreateUpdateRuleDTO(int id, string name, string type, string value, int version = default)
        {
            return new UpdateRuleDTO
            {
                Id = id,
                Name = name,
                Type = type,
                Value = value,
                Version = version
            };
        }

        private void AssertRuleAndDTO(Rule rule, UpdateRuleDTO updateRuleDTO)
        {
            Assert.AreEqual(rule.Id, updateRuleDTO.Id);
            Assert.AreEqual(rule.Name, updateRuleDTO.Name);
            Assert.AreEqual(rule.Type, updateRuleDTO.Type);
            Assert.AreEqual(rule.Value, updateRuleDTO.Value);
        }
    }
}
