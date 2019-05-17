import { Component, OnInit, Inject } from '@angular/core';
import { FileService } from '../../services/file.service';
import { ShareService } from '../../services/share.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ApiResponse } from '../../models/ApiResponse';

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
    
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
