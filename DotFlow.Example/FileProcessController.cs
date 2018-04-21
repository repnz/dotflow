using System;
using System.Text;

namespace DotFlow.Example
{
    public class FileProcessController : IErrorListener
    {
        private readonly Flow _flow;

        public FileProcessController()
        {
            _flow = InitializeFlow();
        }

        public void Start()
        {
            _flow.Start();
        }

        public void Stop()
        {
            _flow.Stop();
        }

        public void Wait()
        {
            _flow.Wait();
        }

        private Flow InitializeFlow()
        {
            // Create FlowBuilder
            var flowBuilder = new FlowBuilder(
                errorListener: this,
                numberOfTasksPerActor: 2,
                maxBufferSize: 1
                );

            // Create source and target
            ISource<FileObject> sourceAdapter = new FileSystemSource("Input", maxBuffer: 2);
            ITarget<FileObject> targetAdapter = new FileSystemTarget("Output");

            // Declare the steps of the flow
            return flowBuilder
                .Source<FileObject>(sourceAdapter)
                .AddActor<ProcessObject>(FileObjectToProcessObject, nameof(FileObjectToProcessObject))
                .AddActor<ProcessObject>(RemoveSpaces, nameof(RemoveSpaces))
                .AddActor<ProcessObject>(DuplicateString, nameof(DuplicateString))
                .AddActor<FileObject>(ProcessObjectToFileObject, nameof(ProcessObjectToFileObject))
                .Target(targetAdapter)
                .Create("File Change String Flow");
        }

        /// <summary>
        /// Step A - get input, validate, and parse into ProcessObject
        /// </summary>
        private ProcessObject FileObjectToProcessObject(FileObject arg)
        {
            Console.WriteLine("FileObjectToProcessObject");

            if (!arg.Filename.EndsWith(".txt"))
            {
                throw new Exception("Filename must end with .txt1!");
            }

            return new ProcessObject(arg.Filename, Encoding.ASCII.GetString(arg.Contents));
        }


        /// <summary>
        /// Step B - Remove spaces from the ProcessObject
        /// </summary>
        private ProcessObject RemoveSpaces(ProcessObject arg)
        {
            Console.WriteLine("RemoveSpaces");
            arg.Value = arg.Value.Replace(" ", "");
            return arg;
        }


        /// <summary>
        /// Step C - Duplicate the string inside ProcessObject
        /// </summary>
        private ProcessObject DuplicateString(ProcessObject obj)
        {
            Console.WriteLine("DuplicateString");
            obj.Value += obj.Value;
            return obj;
        }

        /// <summary>
        /// Step D - Convert the ProcessObject back to FileObject - to write it out
        /// </summary>
        private FileObject ProcessObjectToFileObject(ProcessObject obj)
        {
            Console.WriteLine("ProcessObjectToFileObject");
            return new FileObject(obj.Name, Encoding.ASCII.GetBytes(obj.Value));
        }

        public void OnError(ActorComponent component, string actorName, Exception e)
        {
            Console.WriteLine($"ActorName={actorName} Component={component} Exception={e}");
        }
    }
}
