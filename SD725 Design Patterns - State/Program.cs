using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace fsm
{
    class Program
    {
        static void Main(string[] args)
        {
            var fsm = new FiniteStateMachine();

            while (true)
            {
                Console.WriteLine("Current State: " + fsm.State);
                Console.Write("Enter Event : ");
                string line = Console.ReadLine();
                if (line == "Exit") // Check string
                {
                    break;
                }

                fsm.ProcessEvent((FiniteStateMachine.Events)Enum.Parse(typeof(FiniteStateMachine.Events), line) ) ;
                Console.WriteLine("-------------------------");
            }
        }

        class FiniteStateMachine
        {
            public enum States { Start, Standby, On };
            public States State { get; set; }

            public enum Events { PlugIn, TurnOn, TurnOff, RemovePower };

            private Action[,] fsm;

            public FiniteStateMachine()
            {
                this.fsm = new Action[3, 4] { 
                //PlugIn,       TurnOn,                 TurnOff,            RemovePower
                {this.PowerOn,  null,                   null,               null},              //start
                {null,          this.StandbyWhenOff,    null,               this.PowerOff},     //standby
                {null,          null,                   this.StandbyWhenOn, this.PowerOff} };   //on
            }
            public void ProcessEvent(Events theEvent)
            {
                if (this.fsm[(int)this.State, (int)theEvent] == null)
                {
                    Console.WriteLine("Not valid in this state");
                } 
                else
                {
                    this.fsm[(int)this.State, (int)theEvent].Invoke();
                }

            }

            private void PowerOn() { this.State = States.Standby; }
            private void PowerOff() { this.State = States.Start; }
            private void StandbyWhenOn() { this.State = States.Standby; }
            private void StandbyWhenOff() { this.State = States.On; }
        }
    }
}