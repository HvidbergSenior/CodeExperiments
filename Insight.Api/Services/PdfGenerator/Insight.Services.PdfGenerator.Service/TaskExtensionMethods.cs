using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Services.PdfGenerator.Service
{
    public static class TaskExtensionMethods
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var TCS = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            using (cancellationToken.Register(state =>
            {
                ((TaskCompletionSource<object>)state!).TrySetResult(null!);
            }, TCS))
            {
                var resultTask = await Task.WhenAny(task, TCS.Task);
                if (resultTask == TCS.Task)
                {
                    throw new OperationCanceledException(cancellationToken);
                }
                return await task;
            }
        }
    }
}
