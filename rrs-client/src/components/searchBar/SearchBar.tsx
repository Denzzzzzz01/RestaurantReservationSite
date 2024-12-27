const SearchBar: React.FC<{
    searchQuery: string;
    onSearchChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  }> = ({ searchQuery, onSearchChange }) => (
    <div>
      <label htmlFor="search">Search Restaurants: </label>
      <input
        id="search"
        type="text"
        value={searchQuery}
        onChange={onSearchChange}
        placeholder="Enter restaurant name"
      />
    </div>
  );

export default SearchBar;
  