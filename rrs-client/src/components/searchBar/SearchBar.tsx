import './SearchBar.scss';

interface SearchBarProps {
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  placeholder?: string;
  height?: string; 
  fontSize?: string;
}

const SearchBar: React.FC<SearchBarProps> = ({
  value,
  onChange,
  placeholder = "Search...",
  height = "3rem", 
  fontSize = "1rem", 
}) => {
  return (
    <div className="search-bar" style={{ height }}>
      <i className="fi fi-rr-search search-icon"></i>
      <input
        type="text"
        value={value}
        onChange={onChange}
        placeholder={placeholder}
        className="search-input"
        style={{ fontSize }}
      />
    </div>
  );
};


export default SearchBar;
  