namespace laboCrypto
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btAction = new System.Windows.Forms.Button();
            this.txtData = new System.Windows.Forms.TextBox();
            this.txtNbZero = new System.Windows.Forms.TextBox();
            this.txtRes = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btAction
            // 
            this.btAction.Location = new System.Drawing.Point(284, 396);
            this.btAction.Name = "btAction";
            this.btAction.Size = new System.Drawing.Size(208, 35);
            this.btAction.TabIndex = 0;
            this.btAction.Text = "action";
            this.btAction.UseVisualStyleBackColor = true;
            this.btAction.Click += new System.EventHandler(this.btAction_Click);
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(123, 24);
            this.txtData.MaxLength = 1073741824;
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtData.Size = new System.Drawing.Size(563, 314);
            this.txtData.TabIndex = 1;
            this.txtData.Text = resources.GetString("txtData.Text");
            // 
            // txtNbZero
            // 
            this.txtNbZero.Location = new System.Drawing.Point(190, 356);
            this.txtNbZero.Name = "txtNbZero";
            this.txtNbZero.Size = new System.Drawing.Size(397, 20);
            this.txtNbZero.TabIndex = 2;
            this.txtNbZero.Text = "8";
            // 
            // txtRes
            // 
            this.txtRes.Location = new System.Drawing.Point(146, 448);
            this.txtRes.Multiline = true;
            this.txtRes.Name = "txtRes";
            this.txtRes.Size = new System.Drawing.Size(516, 140);
            this.txtRes.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 600);
            this.Controls.Add(this.txtRes);
            this.Controls.Add(this.txtNbZero);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.btAction);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btAction;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.TextBox txtNbZero;
        private System.Windows.Forms.TextBox txtRes;
    }
}

