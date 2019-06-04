using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace wizardscode.validation
{
    public class ValidationResultCollection
    {
        Dictionary<int, ValidationResult> collection = new Dictionary<int, ValidationResult>();
       
        /// <summary>
        /// Get or create a ValidationResult for a setting and a specific validation test.
        /// </summary>
        /// <param name="settingTest">The name of the setting test this is a result for.</param>
        /// <param name="reportingTest">The name of the ValidationTest that this result is generated for.</param>
        /// <returns>An existing ValidationResult if the test has already been run, or a new validation result with an untested state.</returns>
        public ValidationResult GetOrCreate(string settingTest, string reportingTest)
        {
            ValidationResult result;
            if (!collection.TryGetValue(settingTest.GetHashCode(), out result))
            {
                result = new ValidationResult(settingTest);
                result.ReportingTest.Add(reportingTest);
                AddOrUpdate(result, reportingTest);
            }
            return result;
        }

        /// <summary>
        /// Either updates a result or creates a new one with the given status.
        /// </summary>
        /// <param name="settingTest">The name of the setting test this is a result for.</param>
        /// <param name="reportingTest">The name of the ValidationTest that this result is generated for.</param>
        /// <param name="status">The status of the result.</param>
        public void SetStatus(string settingTest, string reportingTest, ValidationResult.Level status)
        {
            ValidationResult result = GetOrCreate(settingTest, reportingTest);
            result.impact = status;
            AddOrUpdate(result, reportingTest);
        }

        public void AddOrUpdate(ValidationResult result, string reportingTest)
        {
            ValidationResult existing;
            if (collection.TryGetValue(result.id, out existing))
            {
                if (existing.ReportingTest.Count > 1 && existing.ReportingTest.Contains(reportingTest))
                {
                    existing.ReportingTest.Remove(reportingTest);
                    switch (existing.impact)
                    {
                        case ValidationResult.Level.Error:
                            result.impact = ValidationResult.Level.Error;
                            break;
                        case ValidationResult.Level.Warning:
                            if (result.impact == ValidationResult.Level.OK)
                            {
                                result.impact = ValidationResult.Level.Warning;
                            }
                            break;
                    }
                } else
                {
                    Remove(result);
                }
            }
            collection[result.id] = result;
        }

        public void AddOrUpdateAll(ValidationResultCollection results)
        {
            foreach (ValidationResult result in results.collection.Values)
            {
                if (result.impact != ValidationResult.Level.OK)
                {
                    collection[result.id] = result;
                }
            }
        }

        public void Remove(ValidationResult result)
        {
            Remove(result.name);
        }

        public void Remove(string name)
        {
            collection.Remove(name.GetHashCode());
        }

        public int Count
        {
            get { return collection.Count(); }
        }

        public int CountWarning
        {
            get { return collection.Values.Count(x => x.impact == ValidationResult.Level.Warning); }
        }

        public int CountError
        {
            get { return collection.Values.Count(x => x.impact == ValidationResult.Level.Error); }
        }

        public List<ValidationResult> ErrorList
        {
            get { return collection.Values.Where(x => x.impact == ValidationResult.Level.Error).ToList(); }
        }

        public List<ValidationResult> WarningList
        {
            get { return collection.Values.Where(x => x.impact == ValidationResult.Level.Warning).ToList(); }
        }

        public List<ValidationResult> OKList
        {
            get { return collection.Values.Where(x => x.impact == ValidationResult.Level.OK).ToList(); }
        }

        internal void Pass(string testName, string reportingTest)
        {
            Remove(testName);
            AddOrUpdate(new ValidationResult(testName, ValidationResult.Level.OK), reportingTest);
        }
    }
}
