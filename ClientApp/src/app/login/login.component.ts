import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { LoginService } from "../Shared/Services/login.service";
import { CookieService } from "ngx-cookie-service";
import { Router } from "@angular/router";
import { StartupService } from "../Shared/Services/startup.service";
import { ServerSettings } from "../Shared/Interfaces/ServerSettings";
import { first, takeUntil } from "rxjs/operators";

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

  settings: ServerSettings | undefined;

  constructor(
    private loginService: LoginService,
    private cookies: CookieService,
    private router: Router,
    private startupService: StartupService
  ) {
  }

  ngOnInit(): void {
    if(this.cookies.get('refreshToken')) {
      const today = new Date();
      this.router.navigateByUrl(`year/${today.getFullYear()}/month/${today.getMonth() + 1}`);
    }
    this.startupService.settings.pipe(first()).subscribe(x => this.settings = x);
  }

  login(): void {
    const username = this.loginForm.get('username')?.value;
    const password = this.loginForm.get('password')?.value;
    this.loginService.login(username, password);
  }

}
