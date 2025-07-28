import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoryTableComponent } from './ui/story-table.component';

const routes: Routes = [
  { path: 'stories', component: StoryTableComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
