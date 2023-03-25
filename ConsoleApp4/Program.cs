using System;

namespace Workflow
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkflowEngine workflowEngine = new WorkflowEngine();

         
            workflowEngine.RegisterActivity(new UploadActivity());
            workflowEngine.RegisterActivity(new EmailActivity());
            workflowEngine.RegisterActivity(new FileProcessingActivity());

            
            workflowEngine.Start();

            Console.ReadKey();
        }
    }

 
    public abstract class Activity
    {
        public abstract string Name { get; }
        public abstract void Execute();
        public event EventHandler<ActivityEventArgs> ActivityComplete;

        protected void OnActivityComplete(ActivityEventArgs e)
        {
            ActivityComplete?.Invoke(this, e);
        }
    }

 
    public class ActivityEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public ActivityEventArgs(string message)
        {
            Message = message;
        }
    }


    public class WorkflowEngine
    {
        private Queue<Activity> activities;

        public WorkflowEngine()
        {
            activities = new Queue<Activity>();
        }

        public void RegisterActivity(Activity activity)
        {
            activities.Enqueue(activity);
            activity.ActivityComplete += OnActivityComplete;
        }

        public void Start()
        {
            Console.WriteLine("Workflow engine started.");
            ExecuteNextActivity();
        }

        private void OnActivityComplete(object sender, ActivityEventArgs e)
        {
            Console.WriteLine(e.Message);
            ExecuteNextActivity();
        }

        private void ExecuteNextActivity()
        {
            if (activities.Count > 0)
            {
                Activity activity = activities.Dequeue();
                Console.WriteLine($"Executing activity: {activity.Name}");
                activity.Execute();
            }
            else
            {
                Console.WriteLine("Workflow engine completed.");
            }
        }
    }

   
    public class UploadActivity : Activity
    {
        public override string Name => "Upload";

        public override void Execute()
        {
            Console.WriteLine("Uploading file...");
            OnActivityComplete(new ActivityEventArgs("File uploaded."));
        }
    }


    public class EmailActivity : Activity
    {
        public override string Name => "Email";

        public override void Execute()
        {
            Console.WriteLine("Sending email...");
            OnActivityComplete(new ActivityEventArgs("Email sent."));
        }
    }

    public class FileProcessingActivity : Activity
    {
        public override string Name => "File Processing";

        public override void Execute()
        {
            Console.WriteLine("Processing file...");
            OnActivityComplete(new ActivityEventArgs("File processed."));
        }
    }
}
