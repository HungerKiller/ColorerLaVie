import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { User } from 'src/app/Models/User';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass']
})
export class LoginComponent implements OnInit {

  validateForm!: FormGroup;

  submitForm(): void {
    this.getToken(new User(0, "null", "null", this.validateForm.value.username, this.validateForm.value.password, "null", "null"));
    // for (const i in this.validateForm.controls) {
    //   this.validateForm.controls[i].markAsDirty();
    //   this.validateForm.controls[i].updateValueAndValidity();
    // }
  }

  constructor(private userService: UserService, private fb: FormBuilder, private messageService: NzMessageService) { }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      username: [null, [Validators.required]],
      password: [null, [Validators.required]],
      remember: [true]
    });
  }

  getToken(user: User): void {
    this.userService.getToken(user)
      .subscribe({
        next: data => {
          this.messageService.create("success", "login succeed!");
          localStorage.setItem("username", data.username);
          localStorage.setItem("userrole", data.role);
          localStorage.setItem("token", data.token);
        },
        error: error => {
          this.messageService.create("error", error.error);
        }
      });
  }

  get username(): any {
    return localStorage.getItem('username');
  }
  get userrole(): any {
    return localStorage.getItem('userrole');
  }
}