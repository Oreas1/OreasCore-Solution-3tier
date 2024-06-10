using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using OreasServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OreasCore.Custom_Classes
{

    public static class CustomMessage
    {
        public static string ModelValidationFailedMessage(ModelStateDictionary modelState)
        {
            return string.Join(System.Environment.NewLine, modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }
    }

    public class Init_ViewSetupStructure
    {
        [Required]
        public string Controller { get; set; }
        public object WildCard { get; set; }
        public object WildCardDateRange { get; set; }
        public object LoadByCard { get; set; }
        public object Reports { get; set; }
        public object Privilege { get; set; }
        public object Otherdata { get; set; }
    }

    public class MachineProcessInfo
    {
        public int MachineID { get; set; }
        public string ConnectionID { get; set; }
        public string UserName { get; set; }
        public string ForProcess { get; set; }
        public CancellationTokenSource cancellationTokenSource { get; set; }

    }
    public static class MachineProcessStatus
    {
        public static List<MachineProcessInfo> MachineProcessStatusList { get; set; } = new List<MachineProcessInfo>();

    }

    public class PayRunProcessInfo
    {
        public int PayRunID { get; set; }
        public string ConnectionID { get; set; }
        public string UserName { get; set; }
        public string ForProcess { get; set; }
        public CancellationTokenSource cancellationTokenSource { get; set; }

    }

    public static class PayRunProcessStatus
    {
        public static List<PayRunProcessInfo> PayRunProcessStatusList { get; set; } = new List<PayRunProcessInfo>();

    }

    #region Circut example
    //------add in startup class ==> services.AddSingleton<CircuitHandler, TrackingCircuitHandler>(); ------//

    //public class TrackingCircuitHandler : CircuitHandler
    //{
    //    private HashSet<Circuit> circuits = new HashSet<Circuit>();
    //    public event EventHandler CircuitsChanged;

    //    protected virtual void OnCircuitsChanged()
    //    => CircuitsChanged?.Invoke(this, EventArgs.Empty);
    //    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    //    {

    //        return base.OnCircuitOpenedAsync(circuit, cancellationToken);
    //    }
    //    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    //    {
    //        OnCircuitsChanged();
    //        return base.OnCircuitClosedAsync(circuit, cancellationToken);
    //    }
    //    public override Task OnConnectionUpAsync(Circuit circuit,
    //        CancellationToken cancellationToken)
    //    {
    //        circuits.Add(circuit);

    //        return Task.CompletedTask;
    //    }

    //    public override Task OnConnectionDownAsync(Circuit circuit,
    //        CancellationToken cancellationToken)
    //    {



    //        circuits.Remove(circuit);


    //        return Task.CompletedTask;
    //    }

    //    public int ConnectedCircuits => circuits.Count;
    //}

    //----------------------------------xxxxxxxxxxxxxxxxxxxxxx-----------------------//

    /* this code used in blazor class to use circuit
    
     -----------------------------At top where parameters are define inject service to use
      
     *  [Inject]
        protected CircuitHandler circuitHandler { get; set; }

      -----------------------------------------on initialized method register event handler
     *  protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();            
            (circuitHandler as TrackingCircuitHandler).CircuitsChanged +=
                                                    HandleCircuitsChanged;
        }

      -----------------------------------------Make local function to handle this handler
      *  public void HandleCircuitsChanged(object sender, EventArgs args)
        {
           // do your stuff
            InvokeAsync(() => { RemoveResource(); StateHasChanged(); }) ;
        }

       ----------------------------------------un register on dispose method
      *  public void Dispose()
        {
            (circuitHandler as TrackingCircuitHandler).CircuitsChanged -=
                                         HandleCircuitsChanged;
        }
     
     */
    #endregion
}
