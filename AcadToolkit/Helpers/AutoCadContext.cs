using AcadToolkit.Interops;

namespace AcadToolkit.Helpers;
internal class AutoCadContext
{
  public static void Focus() => Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView();

  public static void Run( Action<TransactionScope> action, bool lockDocument = false, bool abortOnError = true )
  {
    Document activeDwg = Application.DocumentManager.MdiActiveDocument;
    DocumentLock? documentLock = null;
    TransactionScope? scope = null;
    try {
      if ( lockDocument )
        documentLock = activeDwg.LockDocument();
      scope = new TransactionScope( activeDwg.Database );
      action( scope );
    }
    catch {
      if ( abortOnError )
        scope?.Abort();
      throw;
    }
    finally {
      scope?.Dispose();
      documentLock?.Dispose();
    }
  }

  public static void Run( Action action, bool lockDocument = false, bool abortOnError = true )
  {
    Document activeDwg = Application.DocumentManager.MdiActiveDocument;
    DocumentLock? documentLock = null;
    try {
      if ( lockDocument )
        documentLock = activeDwg.LockDocument();
      action();
    }
    catch {
      throw;
    }
    finally {
      documentLock?.Dispose();
    }
  }

  public static TResult? GetResult<TResult>( Func<TransactionScope, TResult> func, bool lockDocument = false, bool abortOnError = true )
  {
    Document activeDwg = Application.DocumentManager.MdiActiveDocument;
    TResult? result = default;
    DocumentLock? documentLock = null;
    TransactionScope? scope = null;
    try {
      if ( lockDocument )
        documentLock = activeDwg.LockDocument();
      scope = new TransactionScope( activeDwg.Database );
      result = func( scope );
    }
    catch {
      if ( abortOnError )
        scope?.Abort();
      throw;
    }
    finally {
      scope?.Dispose();
      documentLock?.Dispose();
    }
    return result;
  }

  public static TResult? GetResult<TResult>( Func<TResult> func, bool lockDocument = false, bool abortOnError = true )
  {
    Document activeDwg = Application.DocumentManager.MdiActiveDocument;
    DocumentLock? documentLock = null;
    TResult? result = default;
    try {
      if ( lockDocument )
        documentLock = activeDwg.LockDocument();
      result = func();
    }
    catch {
      throw;
    }
    finally {
      documentLock?.Dispose();
    }
    return result;
  }
}
