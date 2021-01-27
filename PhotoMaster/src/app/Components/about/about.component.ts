import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.sass']
})
export class AboutComponent implements OnInit {

  listOfTech: Tech[] = [
    {
      key: "Frontend",
      tech: "Angular 11, ng-zorro 11, css/sass/scss"
    },
    {
      key: "Backend",
      tech: ".NET Core 3.1, AutoMapper, EntityFrameworkCore, xUnit, Moq, Shouldly"
    },
    {
      key: "Database",
      tech: "SQL Server"
    },
    {
      key: "Version Control",
      tech: "Git"
    }
  ];

  constructor() { }

  ngOnInit(): void {
  }

}

interface Tech {
  key: string;
  tech: string;
}