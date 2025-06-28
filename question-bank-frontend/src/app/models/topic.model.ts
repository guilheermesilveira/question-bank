export interface Topic {
  id: number;
  name: string;
}

export interface AddTopic {
  name: string;
}

export interface UpdateTopic {
  id: number;
  name: string;
}

export interface SearchTopic {
  name?: string;
}
