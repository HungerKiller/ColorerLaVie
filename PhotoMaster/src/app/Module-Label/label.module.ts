import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';

import { LabelComponent } from './label.component';
import { LabelDetailComponent } from './Components/label-detail/label-detail.component';
import { LabelListComponent } from './Components/label-list/label-list.component';

@NgModule({
  declarations: [LabelComponent, LabelDetailComponent, LabelListComponent],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    NzTableModule,
    NzButtonModule,
    NzDividerModule,
    NzPaginationModule,
    NzIconModule,
    NzGridModule,
    NzDrawerModule,
    NzInputModule,
    NzMessageModule,
    NzPopconfirmModule,
    NzToolTipModule
  ]
})
export class LabelModule { }
