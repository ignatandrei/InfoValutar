export function AutoUnsub( constructor ) {

    const original = constructor.prototype.ngOnDestroy;
    console.log('from unsubscribe');
    constructor.prototype.ngOnDestroy = function() {
      // tslint:disable-next-line: forin
      for ( const prop in this ) {
        const property = this[ prop ];
        if ( property && (typeof property.unsubscribe === 'function') ) {
            console.log('unsubscribe !');
            property.unsubscribe();
        }
      }
      console.log('finish unsub');
      
      // tslint:disable-next-line: no-unused-expression
      original && typeof original === 'function' && original.apply(this, arguments);
    };

  }
