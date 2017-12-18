using System;

namespace HdUtilities.Models
{
    public class SyncProgress
    {
        public string StepMessage { get; set; }
        public double StepMinimum { get; set; }
        public double StepMaximum { get; set; }
        public double StepValue { get; set; }

        public string SubStepMessage { get; set; }
        public double SubStepMinimum { get; set; }
        public double SubStepMaximum { get; set; }
        public double SubStepValue { get; set; }

        public SyncProgress()
        {
            StepMessage = null;
            StepMinimum = 0.0;
            StepMaximum = 0.0;
            StepValue = 0.0;
            SubStepMessage = null;
            SubStepMinimum = 0.0;
            SubStepMaximum = 0.0;
            SubStepValue = 0.0;
        }

        public SyncProgress(SyncProgress initial)
        {
            StepMessage = initial.StepMessage;
            StepMinimum = initial.StepMinimum;
            StepMaximum = initial.StepMaximum;
            StepValue = initial.StepValue;
            SubStepMessage = initial.StepMessage;
            SubStepMinimum = initial.StepMinimum;
            SubStepMaximum = initial.StepMaximum;
            SubStepValue = initial.StepValue;
        }

        public void SubmitSubStepUpdate(IProgress<SyncProgress> progress,
            string subStepMessage = null,
            double? subStepValue = null,
            double? subStepMinimum = null,
            double? subStepMaximum = null)
        {
            if (subStepMessage != null)
                SubStepMessage = subStepMessage;

            if (subStepValue.HasValue)
                SubStepValue = subStepValue.Value;

            if (subStepMinimum.HasValue)
                SubStepMinimum = subStepMinimum.Value;

            if (subStepMaximum.HasValue)
                SubStepMaximum = subStepMaximum.Value;

            ReportProgress(progress);
        }

        public void SubmitStepUpdate(IProgress<SyncProgress> progress,
            string stepMessage = null,
            double? stepValue = null,
            double? stepMinimum = null,
            double? stepMaximum = null)
        {
            if (stepMessage != null)
                StepMessage = stepMessage;

            if (stepValue.HasValue)
                StepValue = stepValue.Value;

            if (stepMinimum.HasValue)
                StepMinimum = stepMinimum.Value;

            if (stepMaximum.HasValue)
                StepMaximum = stepMaximum.Value;

            ReportProgress(progress);
        }

        public void SubmitIncrementedStepUpdate(IProgress<SyncProgress> progress, string stepMessage = null)
        {
            if (stepMessage != null)
                StepMessage = stepMessage;

            StepValue++;

            ReportProgress(progress);
        }

        public void SubmitIncrementedSubStepUpdate(IProgress<SyncProgress> progress, string subStepMessage = null)
        {
            if (subStepMessage != null)
                SubStepMessage = subStepMessage;

            SubStepValue++;

            ReportProgress(progress);
        }

        private void ReportProgress(IProgress<SyncProgress> progress)
        {
            progress?.Report(this);
        }
    }
}