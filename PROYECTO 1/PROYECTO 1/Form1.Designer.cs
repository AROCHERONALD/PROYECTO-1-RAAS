namespace PROYECTO_1
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPrompt = new System.Windows.Forms.TextBox();
            this.btnInvestigar = new System.Windows.Forms.Button();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.txtResultados = new System.Windows.Forms.RichTextBox();
            this.btnGenerarInforme = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblEstado = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtPrompt
            // 
            this.txtPrompt.Location = new System.Drawing.Point(30, 50);
            this.txtPrompt.Multiline = true;
            this.txtPrompt.Name = "txtPrompt";
            this.txtPrompt.Size = new System.Drawing.Size(600, 80);
            this.txtPrompt.TabIndex = 0;
            this.txtPrompt.TextChanged += new System.EventHandler(this.txtPrompt_TextChanged);
            // 
            // btnInvestigar
            // 
            this.btnInvestigar.Location = new System.Drawing.Point(30, 150);
            this.btnInvestigar.Name = "btnInvestigar";
            this.btnInvestigar.Size = new System.Drawing.Size(120, 40);
            this.btnInvestigar.TabIndex = 1;
            this.btnInvestigar.Text = "Investigar";
            this.btnInvestigar.UseVisualStyleBackColor = true;
            this.btnInvestigar.Click += new System.EventHandler(this.btnInvestigar_Click);
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(27, 25);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(174, 16);
            this.lblPrompt.TabIndex = 2;
            this.lblPrompt.Text = "Ingrese el tema a investigar:";
            // 
            // txtResultados
            // 
            this.txtResultados.Location = new System.Drawing.Point(30, 210);
            this.txtResultados.Name = "txtResultados";
            this.txtResultados.Size = new System.Drawing.Size(600, 200);
            this.txtResultados.TabIndex = 3;
            this.txtResultados.Text = "";
            // 
            // btnGenerarInforme
            // 
            this.btnGenerarInforme.Location = new System.Drawing.Point(170, 150);
            this.btnGenerarInforme.Name = "btnGenerarInforme";
            this.btnGenerarInforme.Size = new System.Drawing.Size(150, 40);
            this.btnGenerarInforme.TabIndex = 4;
            this.btnGenerarInforme.Text = "Generar Informe";
            this.btnGenerarInforme.UseVisualStyleBackColor = true;
            this.btnGenerarInforme.Click += new System.EventHandler(this.btnGenerarInforme_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(340, 160);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(290, 23);
            this.progressBar.TabIndex = 5;
            // 
            // lblEstado
            // 
            this.lblEstado.Location = new System.Drawing.Point(30, 420);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(600, 23);
            this.lblEstado.TabIndex = 6;
            this.lblEstado.Text = "Esperando acción...";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 460);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnGenerarInforme);
            this.Controls.Add(this.txtResultados);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.btnInvestigar);
            this.Controls.Add(this.txtPrompt);
            this.Name = "Form1";
            this.Text = "Agente de Investigación AI";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPrompt;
        private System.Windows.Forms.Button btnInvestigar;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.RichTextBox txtResultados;
        private System.Windows.Forms.Button btnGenerarInforme;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblEstado;
    }
}
