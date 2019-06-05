import { Component, OnInit, Inject } from '@angular/core';
import { FileService } from '../../services/file.service';
import { ShareService } from '../../services/share.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import * as _ from 'lodash';
import { ApiResponse } from '../../models/ApiResponse';

@Component({
  selector: 'app-upload-image',
  templateUrl: './upload-image.component.html',
  styleUrls: ['./upload-image.component.css']
})
export class UploadImageComponent implements OnInit {

  formData = new FormData();
  file: any;
  loadContent: boolean = true;

  constructor(
    private fileService: FileService,
    private shareService: ShareService,
    public dialogRef: MatDialogRef<UploadImageComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
  }

  onSelectFile(event: any): void {
    this.file = event.target.files[0];
  }

  uploadImage(): void {
    if (this.file) {
      this.formData.append(this.file.name, this.file);
      this.loadContent = false;
      this.fileService.SaveImage(this.formData).subscribe((res: ApiResponse) => {
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
    } else {
      this.shareService.openSnackBar("Upload file is empty", "error");
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
