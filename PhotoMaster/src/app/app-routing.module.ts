import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './Components/admin/admin.component';
import { ContactComponent } from './Components/contact/contact.component';
import { DetailViewComponent } from './Module-Photo/Components/detail-view/detail-view.component';
import { GlobalViewComponent } from './Module-Photo/Components/global-view/global-view.component';
import { HomeViewComponent } from './Module-Photo/Components/home-view/home-view.component';
import { LabelListComponent } from './Module-Label/Components/label-list/label-list.component';
import { PhotoListComponent } from './Module-Photo/Components/photo-list/photo-list.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeViewComponent },
  { path: 'global', component: GlobalViewComponent },
  { path: 'detail', component: DetailViewComponent },
  { path: 'contact', component: ContactComponent },
  {
    path: 'admin',
    component: AdminComponent, // this is the component with the <router-outlet> in the template
    children: [
      {
        path: 'labels', // child route path
        component: LabelListComponent, // child route component that the router renders
      },
      {
        path: 'photos',
        component: PhotoListComponent, // another child route component that the router renders
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
