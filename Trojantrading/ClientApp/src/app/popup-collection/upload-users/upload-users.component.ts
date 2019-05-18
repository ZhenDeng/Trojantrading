import { Component, OnInit, Inject } from '@angular/core';
import { FileService } from '../../services/file.service';
import { ShareService } from '../../services/share.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ApiResponse } from '../../models/ApiResponse';
import * as _ from 'lodash';

@Component({
  selector: 'app-upload-users',
  templateUrl: './upload-users.component.html',
  styleUrls: ['./upload-users.component.css']
})
export class UploadUsersComponent implements OnInit {

  formData = new FormData();
  file: any;
  selectedRole: string;

  constructor(
    private fileService: FileService,
    private shareService: ShareService,
    public dialogRef: MatDialogRef<UploadUsersComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
  }

  onSelectFile(event: any): void {
    this.file = event.target.files[0];
  }

  uploadUsers(): void {
    this.formData.append(this.file.name, this.file);
    console.info(_.lowerCase(this.file.name.split('.')[1]));
    if(_.lowerCase(this.file.name.split('.')[1]) != "xlsx" && _.lowerCase(this.file.name.split('.')[1]) != "xls"){
      this.shareService.showError(".uploadusers", "Please upload a excel file", "right");
    }else{
      this.fileService.UploadUsers(this.formData).subscribe((res: ApiResponse) => {
        if(res.status == "success"){
          this.shareService.showSuccess(".uploadusers", res.message, "right");
        }else{
          this.shareService.showError(".uploadusers", res.message, "right");
        }
      },
        (error: any) => {
          console.info(error);
        });
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
