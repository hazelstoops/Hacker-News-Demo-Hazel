export interface Story {
  title: string;
  text: string;
  url: string;
}

export interface Pagination<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}
