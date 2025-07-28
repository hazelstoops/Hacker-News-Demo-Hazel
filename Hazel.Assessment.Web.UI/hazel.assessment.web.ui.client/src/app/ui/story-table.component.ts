
import { Component, OnInit, signal} from '@angular/core';
import { StoryService } from '../../service/story.service';
import { Pagination, Story } from '../../abstractions';


@Component({
  selector: 'story-table',
  templateUrl: './story-table.component.html',
  styleUrls: ['./story-table.component.scss']
})
export class StoryTableComponent implements OnInit {

  page = signal<number>(1);
  totalCount = signal<number>(500);
  pageSize = signal<number>(10);
  pageSizes = [10, 25, 50];
  public searchTerm = '';
  stories: Story[] = [];

  constructor(private storyService: StoryService) {}

  ngOnInit(): void {
    this.getStories(true);
  }

  hasStories() {
    return this.stories && this.stories.length > 0;
  }

  getStories(fromSearchBar: boolean): void {
    if (this.searchTerm !== "") {
      this.searchStories(fromSearchBar)
      return;
    }
    this.loadStories();
  }

  loadStories(): void {
    this.storyService.getStories(this.page(), this.pageSize())
      .subscribe(
        {
          next: (result) => {
            this.setData(result);
          },
          error: (err) => {
            console.error(err);
          }
        });
  }

  searchStories(fromSearchBar: boolean) {
    if (this.searchTerm !== "") {
      if (fromSearchBar) this.page.set(1);
    }

    this.storyService.search(this.searchTerm, this.page(), this.pageSize())
          .subscribe({
            next: (result) => {
              this.setData(result);
            },
            error: (err) => {
              console.error(err);
            }
          });
        
  }

  clearSearch() {
    this.searchTerm = "";
    this.getStories(true);
  }

  private setData(result: Pagination<Story>) {
    this.stories = result.items;
    this.totalCount.set(result.totalCount);
  }

  storyHasUrl(url?: string) {
    return url != null;
  }

  handlePageChange(event: number): void {
    this.page.set(event);
    this.getStories(false);
  }

  pageChanged(event: number): void {
    this.page.set(event);
    this.getStories(false);
  }

  handlePageSizeChange(event: any): void {
    this.pageSize.set(event.target.value);
    this.page.set(1);
    this.getStories(false);
  }

  searchFieldKeyUp(event: KeyboardEvent): void {
    if (event.key === 'Enter') {
      this.getStories(true);
    }
  }

  refreshList(): void {
    this.getStories(false);
  }
}


