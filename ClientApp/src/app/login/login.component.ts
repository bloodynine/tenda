import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { LoginService } from "../Shared/Services/login.service";
import { CookieService } from "ngx-cookie-service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  })

  constructor(
    private loginService: LoginService,
    private cookies: CookieService,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    if(this.cookies.get('refreshToken')) {
      const today = new Date();
      this.router.navigateByUrl(`year/${today.getFullYear()}/month/${today.getMonth() + 1}`);
    }
  }

  login(): void {
    const username = this.loginForm.get('username')?.value;
    const password = this.loginForm.get('password')?.value;
    this.loginService.login(username, password);
  }

}
