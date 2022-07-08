using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpiceSharp;
using SpiceSharp.Components;
using System.Text.RegularExpressions;
//using SpiceSharp.Entities;
using SpiceSharp.Components.BSIM3Behaviors;
using SpiceSharp.Simulations;
//using SpiceSharp.ParameterSets;
using SpiceSharp.Circuits;

namespace UseSpiceSharp1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private BSIM3 CreateBSIM3(string name, string drain, string gate, string source, string bulk, double w, double l, string model)
		{
			// Create the device
			var e = new BSIM3(name, drain, gate, source, bulk);
			e.SetParameter("w", w);
			e.SetParameter("l", l);
			e.Model = model;
			return e;
		}

		private BSIM1 CreateBSIM1(string name, string drain, string gate, string source, string bulk, double w, double l, string model)
		{
			// Create the device
			var e = new BSIM1(name, drain, gate, source, bulk);
			e.SetParameter("w", w);
			e.SetParameter("l", l);
			e.Model = model;
			return e;
		}

		private BSIM1Model CreateBSIM1Model(string name, string parameters)
		{
			var m = new BSIM1Model(name);
			ApplyParameters(m, parameters);
			return m;
		}

		private BSIM3Model CreateBSIM3Model(string name, string parameters)
		{
			var m = new BSIM3Model(name);
			ApplyParameters(m, parameters);
			return m;
		}

		protected void ApplyParameters(Entity entity, string definition)
		{
			// Get all assignments
			definition = Regex.Replace(definition, @"\s*\=\s*", "=");
			string[] assignments = definition.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var assignment in assignments)
			{
				// Get the name and value
				string[] parts = assignment.Split('=');
				if (parts.Length != 2)
					throw new Exception("Invalid assignment");
				string name = parts[0].ToLower();
				double value = double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);

				// Set the entity parameter
				entity.SetParameter(name, value);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// Build the circuit
			var ckt = new Circuit(
			//				new VoltageSource("V1", "in", "0", new Pulse(0.0, 5.0, 0.01, 1e-3, 1e-3, 0.02, 0.04)),
			//				new Resistor("R1", "in", "out", 10.0e3),
			//				new Capacitor("C1", "out", "0", 1e-6)
			);

			//var V2 = new VoltageSource("V2", "2", "0", new Sine(0.0, 1.0, 1.0e6, 0.0, 0.0, 0.0));
			//var R1 = new Resistor("R1", "5", "0", 100.0);
			//var R2 = new Resistor("R2", "6", "5", 100.0);
			//var L1 = new Inductor("L1", "3", "2", 1.0e-9);

			var V1 = new VoltageSource("V1", "vdd", "0", 3.3);
			var V2 = new VoltageSource("V2", "in", "0", new Pulse(0, 3.3, 1e-6, 1e-9, 0.5e-6, 2e-6, 6e-6));
            var R1 = new Resistor("R1", "vdd", "out", 10.0e3);
            var C1 = new Capacitor("C1", "out", "0", 1e-7);

			var mos_model = new Mosfet1Model("NMOS_DEFAULT_MODEL");
			var bsim1_model = CreateBSIM1Model("BSIM1_DEFAULT_MODEL", "temp=25 muz=600 vdd=5 vfb=-0.3 phi=0.6 k1=0.5 u0=670 x2e=-0.07 mus=1082 n0=0.5 tox=1e-7 mj=0.5 mjsw=0.33 pb=0.8 pbsw=1.0 xpart=1.0"); //new BSIM1Model("BSIM_DEFAULT_MODEL");
			var bsim3_model = CreateBSIM3Model("BSIM3_DEFAULT_MODEL", "tnom=27.0 nch=1.024685e+17 tox=1.00000e-08 xj=1.00000e-07 lint=3.75860e-08 wint=-2.02101528644562e-07 vth0=0.6094574 k1=0.5341038 k2=1.703463e-03 k3=-17.24589 dvt0=0.1767506 dvt1=0.5109418 dvt2=-0.05 nlx=9.979638e-08 w0=1e-6 k3b=4.139039 vsat=97662.05 ua=-1.748481e-09 ub=3.178541e-18 uc=1.3623e-10 rdsw=298.873 u0=307.2991 prwb=-2.24e-4 a0=0.4976366 keta=-2.195445e-02 a1=0.0332883 a2=0.9 voff=-9.623903e-02 nfactor=0.8408191 cit=3.994609e-04 cdsc=1.130797e-04 cdscb=2.4e-5 eta0=0.0145072 etab=-3.870303e-03 dsub=0.4116711 pclm=1.813153 pdiblc1=2.003703e-02 pdiblc2=0.00129051 pdiblcb=-1.034e-3 drout=0.4380235 pscbe1=5.752058e+08 pscbe2=7.510319e-05 pvag=0.6370527 prt=68.7 ngate=1.e20 alpha0=1.e-7 beta0=28.4 prwg=-0.001 ags=1.2 dvt0w=0.58 dvt1w=5.3e6 dvt2w=-0.0032 kt1=-.3 kt2=-.03 at=33000 ute=-1.5 ua1=4.31e-09 ub1=7.61e-18 uc1=-2.378e-10 kt1l=1e-8 wr=1 b0=1e-7 b1=1e-7 dwg=5e-8 dwb=2e-8 delta=0.015 cgdl=1e-10 cgsl=1e-10 cgbo=1e-10 xpart=0.0 cgdo=0.4e-9 cgso=0.4e-9 clc=0.1e-6 cle=0.6 ckappa=0.6"); //new BSIM1Model("BSIM_DEFAULT_MODEL");
																																																			 //var M1 = new Mosfet1("M1", "3", "5", "0", "0", "NMOS_DEFAULT_MODEL");
			var M1 = new Mosfet1("M1", "out", "in", "0", "0", "NMOS_DEFAULT_MODEL");
			var b1 = CreateBSIM1("B1", "out", "in", "0", "0", 100e-6, 100e-6, "BSIM1_DEFAULT_MODEL");
			var b3 = CreateBSIM3("B3", "out", "in", "0", "0", 100e-6, 100e-6, "BSIM3_DEFAULT_MODEL");

			//ckt.Add(V2);
			//ckt.Add(R1);
			//ckt.Add(R2);
			//ckt.Add(L1);
			ckt.Add(mos_model);
			ckt.Add(V1);
			ckt.Add(V2);
			ckt.Add(R1);
			ckt.Add(C1);
			//ckt.Add(bsim1_model);
			ckt.Add(bsim3_model);
			//ckt.Add(b1);
			ckt.Add(b3);
			ckt["B3"].SetParameter("m", 4.0);
			//ckt.Add(M1);

			textBox1.Text = "";
			// Create the simulation
			var tran = new Transient("Tran 1", 1e-9, 9e-6);
			// Make the exports
			//var inputExport = new RealVoltageExport(tran, "3");

			var outputExport = new RealVoltageExport(tran, "out");

			// Simulate
			tran.ExportSimulationData += (sender1, args) =>
			{

				//var input = inputExport.Value;
				var output = outputExport.Value;
				textBox1.Text += args.Time.ToString();
				textBox1.Text += " ";
				textBox1.Text += output.ToString();
				//textBox1.Text += " ";
				//textBox1.Text += input.ToString();
				textBox1.Text += "\r\n";
			};
			tran.Run(ckt);
		}
	}
}
