const PaginationControls: React.FC<{
    pageNumber: number;
    totalPages: number;
    onNext: () => void;
    onPrevious: () => void;
  }> = ({ pageNumber, totalPages, onNext, onPrevious }) => (
    <div>
      <button onClick={onPrevious} disabled={pageNumber === 1}>
        Previous Page
      </button>
      <span>
        Page {pageNumber} of {totalPages}
      </span>
      <button onClick={onNext} disabled={pageNumber >= totalPages}>
        Next Page
      </button>
    </div>
  );
  
export default PaginationControls;