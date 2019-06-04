import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ShareService } from '../../services/share.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HeadInformation } from '../../models/header-info';

@Component({
  selector: 'app-edit-header-infomation',
  templateUrl: './edit-header-infomation.component.html',
  styleUrls: ['./edit-header-infomation.component.css']
})
export class EditHeaderInfomationComponent implements OnInit {

  userFormGroup: FormGroup;
  type: string;

  constructor(private formBuilder: FormBuilder,
    private shareSevice: ShareService,
    public dialogRef: MatDialogRef<EditHeaderInfomationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.userFormGroup = this.formBuilder.group({
      imagePath: new FormControl("", Validators.compose([Validators.required])),
      content: new FormControl("")
    });
  }

  ngOnInit() {
    if(this.data.type == "Update"){
      this.userFormGroup.get("imagePath").setValue(this.data.headInfo.imagePath);
      this.userFormGroup.get("content").setValue(this.data.headInfo.content);
    }
  }

  updateHeadInfomation(): void{
    if(this.userFormGroup.valid){
      this.dialogRef.close(this.userFormGroup.value);
    }else{
      this.shareSevice.showError(".imagePathValidate", "Please enter image path", "left");
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
