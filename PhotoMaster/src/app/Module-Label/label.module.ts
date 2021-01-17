import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzGridModule } from 'ng-zorro-antd/grid';

import { LabelComponent } from './label.component';
import { LabelDetailComponent } from './Components/label-detail/label-detail.component';
import { LabelListComponent } from './Components/label-list/label-list.component';

@NgModule({
  declarations: [LabelComponent, LabelDetailComponent, LabelListComponent],
  imports: [
    CommonModule,
    RouterModule,
    NzTableModule,
    NzButtonModule,
    NzDividerModule,
    NzPaginationModule,
    NzIconModule,
    NzGridModule
  ]
})
export class LabelModule { }
