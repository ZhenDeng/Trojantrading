import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FileService } from '../../services/file.service';
import { ApiResponse } from '../../models/ApiResponse';
import { ShareService } from '../../services/share.service';
import * as _ from 'lodash';

@Component({
  selector: 'app-upload-pdf',
  templateUrl: './upload-pdf.component.html',
  styleUrls: ['./upload-pdf.component.css']
})
export class UploadPdfComponent implements OnInit {

  formData = new FormData();
  file: any;
  selectedRole: string;
  loadContent: boolean = true;

  constructor(
    private fileService: FileService,
    private shareService: ShareService,
    public dialogRef: MatDialogRef<UploadPdfComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
    this.selectedRole = "agent";
  }

  onSelectFile(event: any): void {
    this.file = event.target.files[0];
  }

  uploadPdf(): void {
    if(this.file){
      this.formData.append(this.file.name, this.file);
      if(_.lowerCase(this.file.name.split('.')[1]) != "pdf"){
        this.shareService.openSnackBar("Please upload a pdf file", "error");
      }else{
        this.loadContent = false;
        this.fileService.SavePdf(this.selectedRole, this.formData).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareService.openSnackBar(res.message, "success");
            setTimeout(() => {
              this.loadContent = true;
              this.dialogRef.close();
            }, 1500);
          } else {
            this.loadContent = true;
            this.shareService.openSnackBar(res.message, "error");
          }
        },
          (error: any) => {
            console.info(error);
            this.loadContent = true;
          });
      }
    }else{
      this.shareService.openSnackBar("Upload file is empty", "error");
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
