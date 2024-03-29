﻿using Repositories;
using Repositories.Models;
using Repositories.UnitOfWork;
using Resources;
using Services.DTO;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RuleService : IRuleService
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository _genericRepository;
        private IRuleRepository _ruleRepository;

        private UpdateRuleDTO MapRuleToUpdateRuleDTO(Rule rule)
        {
            return new UpdateRuleDTO
            {
                Id = rule.Id,
                Name = rule.Name,
                Type = rule.Type,
                Value = rule.Value,
                Version = rule.Version
            };
        }
        private bool IsAbleToCast(string type, string value)
        {
            type = type.ToLower();
            if (type == "int")
            {
                try
                {
                    Convert.ToInt32(value);
                }
                catch (FormatException)
                {
                    return false;
                }
            }
            if (type == "float")
            {
                try
                {
                    Convert.ToSingle(value);
                }
                catch (FormatException)
                {
                    return false;
                }
            }
            return true;
        }

        public RuleService(IUnitOfWork unitOfWork, IGenericRepository genericRepository, IRuleRepository ruleRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
            _ruleRepository = ruleRepository;
        }
        
        public IList<UpdateRuleDTO> FindAllRules()
        {
            IList<Rule> rules;
            using (_unitOfWork.Start())
            {
                rules = _ruleRepository.FindAllRules();
            }

            IList<UpdateRuleDTO> updateRuleDTOs = new List<UpdateRuleDTO>();
            foreach (Rule rule in rules)
            {
                updateRuleDTOs.Add(MapRuleToUpdateRuleDTO(rule));
            }
            return updateRuleDTOs;
        }

        public void UpdateRule(UpdateRuleDTO updateRuleDTO)
        {
            Rule rule;
            using (_unitOfWork.Start())
            {
                rule = _ruleRepository.FindRuleById(updateRuleDTO.Id);
            }
            if (rule == null)
            {
                throw new ObjectNotExistsException(Resource.Rule, Resource.Id, updateRuleDTO.Id);
            }
            if (rule.Version != updateRuleDTO.Version)
            {
                throw new ObjectHasBeenUpdatedException(Resource.Rule, Resource.Id, updateRuleDTO.Id);
            }
            if (!IsAbleToCast(rule.Type, updateRuleDTO.Value))
            {
                throw new UnableToCastException();
            }

            rule.Value = updateRuleDTO.Value;
            using (_unitOfWork.Start())
            {
                _genericRepository.Update(rule);
                _unitOfWork.Commit();
            }
        }

        public int GetMaxNumberOfStudentEachClass()
        {
            using (_unitOfWork.Start())
            {
                return Convert.ToInt32(_ruleRepository.FindRuleById(1000).Value);
            }
        }
        public int GetMinimumAge()
        {
            using (_unitOfWork.Start())
            {
                return Convert.ToInt32(_ruleRepository.FindRuleById(1001).Value);
            }
        }
        public int GetMaximumAge()
        {
            using (_unitOfWork.Start())
            {
                return Convert.ToInt32(_ruleRepository.FindRuleById(1002).Value);
            }
        }
        public float GetMinimumPassScored()
        {
            using (_unitOfWork.Start())
            {
                return Convert.ToSingle(_ruleRepository.FindRuleById(1003).Value);
            }
        }
    }
}
