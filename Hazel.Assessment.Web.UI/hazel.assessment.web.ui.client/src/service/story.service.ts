import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Pagination, Story } from '../abstractions';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root' 
})
export class StoryService {
   constructor(private http: HttpClient) { }

  getStories(pageNumber: number, pageSize: number): Observable<Pagination<Story>> { 
    const params = {
      pageNumber: pageNumber,
      pageSize: pageSize
     };

    let stories = this.http.get<Pagination<Story>>('/story', { params: params });    
     return stories;
  }

  search(term: string, pageNumber: number, pageSize: number): Observable<Pagination<Story>> { 
    const params = {
      term: term,
      pageNumber: pageNumber,
      pageSize: pageSize
    };
   
    let stories = this.http.get<Pagination<Story>>('/story/search', { params: params })
    return stories;
  }
}
