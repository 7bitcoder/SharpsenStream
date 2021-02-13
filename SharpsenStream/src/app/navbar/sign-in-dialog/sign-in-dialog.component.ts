import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


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
    @Inject(MAT_DIALOG_DATA) data) {}

  ngOnInit(): void {
    
  }

  finish(result: boolean) {
    this.dialogRef.close(result);
  }

  signIn(username: string, password: string) {
    
  }

  createAccount(){

  }

}
