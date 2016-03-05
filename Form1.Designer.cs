namespace YouTubeDownloader {
  sealed partial class Form1 {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose (bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent () {
      this.label1 = new System.Windows.Forms.Label();
      this.textfieldLink = new System.Windows.Forms.TextBox();
      this.buttonStart = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.panelWork = new System.Windows.Forms.Panel();
      this.statusCompress = new System.Windows.Forms.Label();
      this.statusExtract = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.labelDone = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.textfieldSize = new System.Windows.Forms.Label();
      this.textfieldLength = new System.Windows.Forms.Label();
      this.textfieldTitle = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.procentDownload = new System.Windows.Forms.Label();
      this.buttonAbort = new System.Windows.Forms.Button();
      this.progressDownload = new System.Windows.Forms.ProgressBar();
      this.label4 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.panelSetup = new System.Windows.Forms.Panel();
      this.panelWork.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.panelSetup.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(73, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Youtube-Link:";
      // 
      // textfieldLink
      // 
      this.textfieldLink.Location = new System.Drawing.Point(6, 29);
      this.textfieldLink.Name = "textfieldLink";
      this.textfieldLink.Size = new System.Drawing.Size(242, 20);
      this.textfieldLink.TabIndex = 1;
      // 
      // buttonStart
      // 
      this.buttonStart.Location = new System.Drawing.Point(254, 27);
      this.buttonStart.Name = "buttonStart";
      this.buttonStart.Size = new System.Drawing.Size(59, 23);
      this.buttonStart.TabIndex = 3;
      this.buttonStart.Text = "Start";
      this.buttonStart.UseVisualStyleBackColor = true;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(7, 81);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(181, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Schritt 2: Video wird heruntergeladen";
      // 
      // panelWork
      // 
      this.panelWork.Controls.Add(this.statusCompress);
      this.panelWork.Controls.Add(this.statusExtract);
      this.panelWork.Controls.Add(this.label5);
      this.panelWork.Controls.Add(this.labelDone);
      this.panelWork.Controls.Add(this.groupBox1);
      this.panelWork.Controls.Add(this.procentDownload);
      this.panelWork.Controls.Add(this.buttonAbort);
      this.panelWork.Controls.Add(this.progressDownload);
      this.panelWork.Controls.Add(this.label4);
      this.panelWork.Controls.Add(this.label3);
      this.panelWork.Location = new System.Drawing.Point(11, 79);
      this.panelWork.Name = "panelWork";
      this.panelWork.Size = new System.Drawing.Size(320, 178);
      this.panelWork.TabIndex = 5;
      // 
      // statusCompress
      // 
      this.statusCompress.AutoSize = true;
      this.statusCompress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.statusCompress.Location = new System.Drawing.Point(193, 123);
      this.statusCompress.Name = "statusCompress";
      this.statusCompress.Size = new System.Drawing.Size(22, 13);
      this.statusCompress.TabIndex = 15;
      this.statusCompress.Text = "OK";
      // 
      // statusExtract
      // 
      this.statusExtract.AutoSize = true;
      this.statusExtract.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.statusExtract.Location = new System.Drawing.Point(193, 102);
      this.statusExtract.Name = "statusExtract";
      this.statusExtract.Size = new System.Drawing.Size(22, 13);
      this.statusExtract.TabIndex = 14;
      this.statusExtract.Text = "OK";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(7, 123);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(150, 13);
      this.label5.TabIndex = 13;
      this.label5.Text = "Schritt 4: Komprimiere als MP3";
      // 
      // labelDone
      // 
      this.labelDone.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.labelDone.AutoSize = true;
      this.labelDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelDone.ForeColor = System.Drawing.Color.Green;
      this.labelDone.Location = new System.Drawing.Point(7, 157);
      this.labelDone.Name = "labelDone";
      this.labelDone.Size = new System.Drawing.Size(43, 13);
      this.labelDone.TabIndex = 12;
      this.labelDone.Text = "Fertig.";
      this.labelDone.Visible = false;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.textfieldSize);
      this.groupBox1.Controls.Add(this.textfieldLength);
      this.groupBox1.Controls.Add(this.textfieldTitle);
      this.groupBox1.Controls.Add(this.label9);
      this.groupBox1.Controls.Add(this.label8);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Location = new System.Drawing.Point(6, 7);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(310, 66);
      this.groupBox1.TabIndex = 11;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Schritt 1: Video-Informationen abrufen";
      // 
      // textfieldSize
      // 
      this.textfieldSize.AutoSize = true;
      this.textfieldSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textfieldSize.Location = new System.Drawing.Point(50, 50);
      this.textfieldSize.MaximumSize = new System.Drawing.Size(100, 0);
      this.textfieldSize.MinimumSize = new System.Drawing.Size(100, 0);
      this.textfieldSize.Name = "textfieldSize";
      this.textfieldSize.Size = new System.Drawing.Size(100, 12);
      this.textfieldSize.TabIndex = 7;
      // 
      // textfieldLength
      // 
      this.textfieldLength.AutoSize = true;
      this.textfieldLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textfieldLength.Location = new System.Drawing.Point(50, 35);
      this.textfieldLength.MaximumSize = new System.Drawing.Size(100, 0);
      this.textfieldLength.MinimumSize = new System.Drawing.Size(100, 0);
      this.textfieldLength.Name = "textfieldLength";
      this.textfieldLength.Size = new System.Drawing.Size(100, 12);
      this.textfieldLength.TabIndex = 6;
      // 
      // textfieldTitle
      // 
      this.textfieldTitle.AutoSize = true;
      this.textfieldTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textfieldTitle.Location = new System.Drawing.Point(50, 20);
      this.textfieldTitle.MaximumSize = new System.Drawing.Size(250, 12);
      this.textfieldTitle.MinimumSize = new System.Drawing.Size(250, 12);
      this.textfieldTitle.Name = "textfieldTitle";
      this.textfieldTitle.Size = new System.Drawing.Size(250, 12);
      this.textfieldTitle.TabIndex = 5;
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label9.Location = new System.Drawing.Point(7, 50);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(34, 12);
      this.label9.TabIndex = 2;
      this.label9.Text = "Größe:";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label8.Location = new System.Drawing.Point(7, 35);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(33, 12);
      this.label8.TabIndex = 1;
      this.label8.Text = "Länge:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(7, 20);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(25, 12);
      this.label2.TabIndex = 0;
      this.label2.Text = "Titel:";
      // 
      // procentDownload
      // 
      this.procentDownload.AutoSize = true;
      this.procentDownload.Location = new System.Drawing.Point(275, 82);
      this.procentDownload.MaximumSize = new System.Drawing.Size(40, 12);
      this.procentDownload.MinimumSize = new System.Drawing.Size(34, 12);
      this.procentDownload.Name = "procentDownload";
      this.procentDownload.Size = new System.Drawing.Size(34, 12);
      this.procentDownload.TabIndex = 9;
      this.procentDownload.Text = "0 %";
      this.procentDownload.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // buttonAbort
      // 
      this.buttonAbort.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.buttonAbort.Location = new System.Drawing.Point(125, 151);
      this.buttonAbort.Name = "buttonAbort";
      this.buttonAbort.Size = new System.Drawing.Size(90, 24);
      this.buttonAbort.TabIndex = 8;
      this.buttonAbort.Text = "Abbrechen";
      this.buttonAbort.UseVisualStyleBackColor = true;
      // 
      // progressDownload
      // 
      this.progressDownload.Location = new System.Drawing.Point(193, 81);
      this.progressDownload.Name = "progressDownload";
      this.progressDownload.Size = new System.Drawing.Size(79, 15);
      this.progressDownload.Step = 1;
      this.progressDownload.TabIndex = 6;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(7, 102);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(141, 13);
      this.label4.TabIndex = 5;
      this.label4.Text = "Schritt 3: Extrahiere Tonspur";
      // 
      // label7
      // 
      this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.label7.AutoSize = true;
      this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label7.ForeColor = System.Drawing.SystemColors.ControlDark;
      this.label7.Location = new System.Drawing.Point(224, 260);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(110, 12);
      this.label7.TabIndex = 6;
      this.label7.Text = "Version 0.03 - 04.03.2016";
      // 
      // panelSetup
      // 
      this.panelSetup.Controls.Add(this.label1);
      this.panelSetup.Controls.Add(this.textfieldLink);
      this.panelSetup.Controls.Add(this.buttonStart);
      this.panelSetup.Location = new System.Drawing.Point(11, 9);
      this.panelSetup.Name = "panelSetup";
      this.panelSetup.Size = new System.Drawing.Size(320, 61);
      this.panelSetup.TabIndex = 7;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(340, 275);
      this.Controls.Add(this.panelSetup);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.panelWork);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Form1";
      this.ShowIcon = false;
      this.Text = "YouTube → MP3";
      this.panelWork.ResumeLayout(false);
      this.panelWork.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.panelSetup.ResumeLayout(false);
      this.panelSetup.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textfieldLink;
    private System.Windows.Forms.Button buttonStart;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Panel panelWork;
    private System.Windows.Forms.Button buttonAbort;
    private System.Windows.Forms.ProgressBar progressDownload;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label procentDownload;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Panel panelSetup;
    private System.Windows.Forms.Label textfieldSize;
    private System.Windows.Forms.Label textfieldTitle;
    private System.Windows.Forms.Label textfieldLength;
    private System.Windows.Forms.Label labelDone;
    private System.Windows.Forms.Label statusCompress;
    private System.Windows.Forms.Label statusExtract;
    private System.Windows.Forms.Label label5;
  }
}

