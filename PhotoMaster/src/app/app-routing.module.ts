import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './Components/admin/admin.component';
import { AboutComponent } from './Components/about/about.component';
import { GlobalViewComponent } from './Module-Photo/global-view/global-view.component';
import { HomeViewComponent } from './Module-Photo/home-view/home-view.component';
import { LabelListComponent } from './Module-Label/label-list/label-list.component';
import { PhotoListComponent } from './Module-Photo/photo-list/photo-list.component';
import { PhotoWallViewComponent } from './Module-Photo/photo-wall-view/photo-wall-view.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeViewComponent },
  { path: 'global', component: GlobalViewComponent },
  { path: 'wall', component: PhotoWallViewComponent },
  { path: 'about', component: AboutComponent },
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
