import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginModel } from '../../Models/LoginModel';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient) { }

  private Response: boolean = false;
   


  public Login(LoginModel: LoginModel) {
    return this.http.post(environment.BaseUrl + "api/Login/Login", LoginModel);
  }

  public GetUserIdByToken(): string {
    let Token = "";

    Token = localStorage.getItem("Token");
    let TokenObject = JSON.parse(atob(Token.split('.')[1]));
    return TokenObject.UserId;
  }

  public GetUserNameByToken(): string {
    let Token = "";

    Token = localStorage.getItem("Token");
    let TokenObject = JSON.parse(atob(Token.split('.')[1]));
    return TokenObject.UserName;
  }

  public GetEmailByToken(): string {
    let Token = "";

    Token = localStorage.getItem("Token");
    let TokenObject = JSON.parse(atob(Token.split('.')[1]));
    return TokenObject.Email;
  }


   
  public IsLoged(): boolean {

    let Token = "";

    if (localStorage.getItem("Token")) {

      Token = localStorage.getItem("Token");
      let TokenObject = JSON.parse(atob(Token.split('.')[1]));

      if ((Token.length < 6)) {
        this.Response = false;
        this.LogOut();
      }

      let ExpireToken = Number(TokenObject.exp);
      let Expire = new Date(ExpireToken * 1000);

      if (Expire < new Date()) {
        this.Response = false;
        this.LogOut();
      }
      else {
        this.Response = true;
      }
    }

    else {
      this.Response = false;
      this.LogOut();
    }


    return this.Response;
  }

  public LogOut() {
    localStorage.removeItem("Token");
    localStorage.removeItem("ExpirationToken"); 
  }
}
