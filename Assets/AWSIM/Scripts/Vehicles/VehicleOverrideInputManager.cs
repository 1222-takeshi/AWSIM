using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWSIM
{
    /// <summary>
    /// Controls vehicle override inputs.
    /// Inputs are applied to the vehicle by making final adjustments 
    /// to the values of AutonomousInput and ManuallyInput according to ControlMode.
    /// </summary>
    public class VehicleOverrideInputManager : MonoBehaviour
    {

        /// <summary>
        /// Inputs used during Autonomous driving.
        /// </summary>
        public VehicleInputBase AutonomousInput;

        /// <summary>
        /// Inputs used during Manually driving. such as keyboard, controller, etc.
        /// </summary>
        public VehicleInputBase ManuallyInput;

        /// <summary>
        /// ControlMode applied to the vehicle.
        /// </summary>
        public VehicleControlMode ControlMode { get; set; } = VehicleControlMode.AUTONOMOUS;

        [SerializeField] Vehicle vehicle;

        private void FixedUpdate()
        {
            ApplyInput();
        }

        void ApplyInput()
        {
            VehicleInputBase.InputArg inputArg = new VehicleInputBase.InputArg(ControlMode, AutonomousInput.SteeringInput);

            // Update new input for Autonomous and Manually Inputs.
            AutonomousInput.OnUpdate(inputArg);
            ManuallyInput.OnUpdate(inputArg);


            // If override input is present, switch new ControlMode.
            if (ManuallyInput.Overridden)
            {
                ControlMode = ManuallyInput.NewControlMode;
            }

            // Apply input to the vehicle according to ControlMode.
            if (ControlMode == VehicleControlMode.AUTONOMOUS)
            {
                vehicle.AccelerationInput = AutonomousInput.AccelerationInput;
                vehicle.SteerAngleInput = AutonomousInput.SteeringInput;
                vehicle.AutomaticShiftInput = AutonomousInput.ShiftInput;
                vehicle.SignalInput = AutonomousInput.TurnSignalInput;
            }
            else if (ControlMode == VehicleControlMode.MANUAL)
            {
                vehicle.AccelerationInput = ManuallyInput.AccelerationInput;
                vehicle.SteerAngleInput = ManuallyInput.SteeringInput;
                vehicle.AutomaticShiftInput = ManuallyInput.ShiftInput;
                vehicle.SignalInput = ManuallyInput.TurnSignalInput;
            }
        }
    }
}