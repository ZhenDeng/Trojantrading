import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FileService } from '../../services/file.service';

@Component({
  selector: 'app-upload-pdf',
  templateUrl: './upload-pdf.component.html',
  styleUrls: ['./upload-pdf.component.css']
})
export class UploadPdfComponent implements OnInit {

  formData = new FormData();
  file: any;

  constructor(
    private fileService: FileService,
    public dialogRef: MatDialogRef<UploadPdfComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
  }

  onSelectFile(event: any): void {
    this.file = event.target.files[0];
  }

  uploadPdf(): void{

    this.formData.append(this.file.name, this.file);
 
    this.fileService.SavePdf(this.formData)
      .subscribe((res) => {
        console.info(res);
      },
        (error: any) => {
          console.info(error);
        });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
