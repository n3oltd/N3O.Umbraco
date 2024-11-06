export function FieldError({message}: {message: string}) {
  return (
    <div className="n3o-checkout__message active">
      <p className="n3o-detail">{message}</p>
    </div>
  );
}