import { Component, OnInit, Input } from '@angular/core';
import { Label } from '../../Models/Label';
import { LabelService } from '../../Services/label.service';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-label-detail',
  templateUrl: './label-detail.component.html',
  styleUrls: ['./label-detail.component.sass']
})
export class LabelDetailComponent implements OnInit {

  labelId : number;
  labelName : string;
  title : string;
  visible : boolean;

  constructor(private labelService : LabelService, private message: NzMessageService) { }

  ngOnInit(): void {
  }

  close() : void{
    this.visible = false;
  }

  submit() : void{
    if(this.title == "Update"){
      this.labelService.putLabel(this.labelId, { Id : this.labelId, Name : this.labelName})
      .subscribe({
            next: data => {
                this.labelName = data.Name;
                this.message.create("success", "Update succeed!");
                this.close();
            },
            error: error => {
                this.message.create("error", error.error);
            }
        });
    }
    else if(this.title == "Create")
    {
      this.labelService.postLabel({ Id : 0, Name : this.labelName})
      .subscribe({
            next: data => {
                this.labelName = data.Name;
                this.message.create("success", "Create succeed!");
                this.close();
            },
            error: error => {
                this.message.create("error", error.error);
            }
        });
    }
  }
}
