using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class TaskRunner
{
    private readonly Func<CancellationToken, Task> taskDelegate;
    private CancellationTokenSource cts = new CancellationTokenSource();
    private Task? task;
    public bool IsRunning { get; private set; }

    public TaskRunner(Func<CancellationToken, Task> taskDelegate)
    {
        this.taskDelegate = taskDelegate ?? throw new ArgumentNullException(nameof(taskDelegate));
    }

    public async Task RunTask(CancellationToken cancellationToken)
    {
        try
        {
            await taskDelegate(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            //Console.WriteLine("Task was canceled.");
        }
        //catch (Exception ex)
        //{
        //    //Console.WriteLine($"An error occurred: {ex.Message}");
        //}
    }
    public void Start(bool executeOnce =false)
    {
        if (this.task is not null && this.task.Status != TaskStatus.Faulted)
        {
            return;
        }

        if (this.task != null && this.task.Status == TaskStatus.Faulted)
        {
            this.task = null;
        } //Task.Run(() => videoStreamControler.UdpTaskRunner.Start());
        this.task = Task.Run(() => this.DoWorkAsync(executeOnce));
        //this.task = this.DoWorkAsync(executeOnce);
    }
    private async Task DoWorkAsync(bool executeOnce = false)
    {
        try
        {
            IsRunning = true;
            if (this.cts.IsCancellationRequested)
            {
                this.cts = new CancellationTokenSource();
            }
                        
            while (!this.cts.IsCancellationRequested && IsRunning)
            {
                await this.taskDelegate(this.cts.Token);                
                
                if (executeOnce)
                {
                 
                    break;
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        //catch (Exception ex)
        //{
           
        //}
        finally
        {
            IsRunning = false;
            this.task = null;
        }
    }
    public async Task StopAsync()
    {
        this.IsRunning = false;

        if (this.task is null)
        {
            return;
        }

        try
        {
            this.cts.Cancel();

            if (this.task is not null)
            {
                await this.task;
            }
            this.cts.Dispose();
        }
        finally
        {           
            this.task = null;
        }
    }
}