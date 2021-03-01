import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserService } from 'src/app/shared/user.service';


export interface DialogData {
}

@Component({
  selector: 'app-sign-in-dialog',
  templateUrl: './sign-in-dialog.component.html',
  styleUrls: ['./sign-in-dialog.component.scss']
})
export class SignInDialogComponent implements OnInit {

  model: any = {};
  loading = false;
  loginData = null
  constructor(
    private dialogRef: MatDialogRef<SignInDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data,
    private user$: UserService) {}

  ngOnInit(): void {
    
  }

  finish(result: boolean) {
    this.dialogRef.close(result);
  }

  signIn(username: string, password: string) {
    this.user$.login(username, password).subscribe( result => {
      if( result ) {
        const user = this.user$.getUser();
        this.dialogRef.close(user);
      }
    })
  }

  createAccount(){

  }

}
