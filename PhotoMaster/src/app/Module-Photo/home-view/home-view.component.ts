import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/Models/User';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-home-view',
  templateUrl: './home-view.component.html',
  styleUrls: ['./home-view.component.sass']
})
export class HomeViewComponent implements OnInit {

  array = ["../../../assets/images/1.jpg", "../../../assets/images/2.jpg", "../../../assets/images/3.jpg",
    "../../../assets/images/4.jpg", "../../../assets/images/5.jpg", "../../../assets/images/6.jpg", "../../../assets/images/7.jpg"];

  index = 0;
  colors = ["#A93226", "#E67E22", "#F1C40F", "#2ECC71", "#3498DB", "#2471A3", "#8E44AD"];
  styles = {
    fontFamily: "'Alex Brush', cursive",
    fontSize: "50px",
    color: "#A93226"
  }

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.setStyle();
    this.loginAsVisitor();
  }

  setStyle() {
    setInterval(() => {
      this.index++;
      if (this.index >= this.colors.length)
        this.index = this.index - this.colors.length;
      this.styles.color = this.colors[this.index];
    }, 2000);
  }

  loginAsVisitor(): void {
    localStorage.clear();
    if(localStorage.getItem("token") != null)
      return;
    this.userService.getToken(new User(0, "null", "null", "visitor", "visitor", "null", "null"))
      .subscribe({
        next: data => {
          localStorage.setItem("username", data.username);
          localStorage.setItem("userrole", data.role);
          localStorage.setItem("token", data.token);
        },
        error: error => {
          console.log(error.error);
        }
      });
  }
}