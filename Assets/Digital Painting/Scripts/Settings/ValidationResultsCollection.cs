using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WizardsCode.Plugin;

namespace WizardsCode.Validation
{
    public class ValidationResultCollection
    {
        Dictionary<int, ValidationResult> collection = new Dictionary<int, ValidationResult>();

        /// <summary>
        /// Search for the highest priority result that is either an Error or a Warning that
        /// is not being ignored.
        /// If there are no errors or warnings that are not ignored returns null.
        /// </summary>
        /// <param name="exclude">A list of test names that should be ignored.</param>
        /// <returns>Highest priority error or warning (not ignored) or null if none exists.</returns>
        public ValidationResult GetHighestPriorityErrorOrWarning(List<string> exclude)
        {
            IEnumerable<KeyValuePair<int, ValidationResult>> candidates = collection.Where(z => z.Value.impact == ValidationResult.Level.Error && !exclude.Contains(z.Value.name));

            if (candidates.Count() > 0)
            {
                return candidates.First().Value;
            }

            candidates = collection.Where(z => z.Value.impact == ValidationResult.Level.Warning && !exclude.Contains(z.Value.name));
            if (candidates.Count() > 0)
            {
                return candidates.First().Value;
            }

            return null;
        }
       
        /// <summary>
        /// Get or create a ValidationResult for a setting and a specific validation test.
        /// </summary>
        /// <param name="settingTest">The name of the setting test this is a result for.</param>
        /// <param name="reportingTest">The name of the ValidationTest that this result is generated for.</param>
        /// <returns>An existing ValidationResult if the test has already been run, or a new validation result with an untested state.</returns>
        public ValidationResult GetOrCreate(string settingTest, AbstractPluginManager pluginManager, string reportingTest)
        {
            ValidationResult result;
            if (!collection.TryGetValue(settingTest.GetHashCode(), out result))
            {
                result = new ValidationResult(settingTest, pluginManager);
                result.ReportingTest.Add(reportingTest);
                AddOrUpdate(result, reportingTest);
            }
            return result;
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
                if (result.impact == ValidationResult.Level.OK)
                {
                    result.RemoveCallbacks();
                }
                collection[result.id] = result;
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

        public int CountOK
        {
            get { return collection.Values.Count(x => x.impact == ValidationResult.Level.OK); }
        }

        /// <summary>
        /// Get a List of OK results.
        /// </summary>
        /// <param name="exclude">A list of result names that should be excluded from the result set.</param>
        /// <returns>All OK results not in the exclude list.</returns>
        public List<ValidationResult> GetOKs(List<String> exclude)
        {
            return collection.Values.Where(z => z.impact == ValidationResult.Level.OK && !exclude.Contains(z.name)).ToList();
        }

        public int CountWarning
        {
            get { return collection.Values.Count(x => x.impact == ValidationResult.Level.Warning); }
        }

        /// <summary>
        /// Get a List of Warning results.
        /// </summary>
        /// <param name="exclude">A list of result names that should be excluded from the result set.</param>
        /// <returns>All Warning results not in the exclude list.</returns>
        public List<ValidationResult> GetWarnings(List<String> exclude)
        {
            return collection.Values.Where(z => z.impact == ValidationResult.Level.Warning && !exclude.Contains(z.name)).ToList();
        }

        public int CountError
        {
            get { return collection.Values.Count(x => x.impact == ValidationResult.Level.Error); }
        }

        /// <summary>
        /// Get List of Error results.
        /// </summary>
        /// <param name="exclude">A list of result names that should be excluded from the result set.</param>
        /// <returns>All Error results not in the exclude list.</returns>
        public List<ValidationResult> GetErrors(List<String> exclude)
        {
            return collection.Values.Where(z => z.impact == ValidationResult.Level.Error && !exclude.Contains(z.name)).ToList();
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

        internal void Pass(string testName, AbstractPluginManager pluginManager, string reportingTest)
        {
            Remove(testName);
            AddOrUpdate(new ValidationResult(testName, pluginManager, ValidationResult.Level.OK), reportingTest);
        }
    }
}
