export function FieldError({message}: {message: string}) {
  return (
    <div className="active checkout__message">
      <p className="detail">{message}</p>
    </div>
  );
}